import argparse
from pathlib import Path
import sys
import pandas as pd


def main(args=None):
    try:
        if args is None:
            parser = create_parser()
            args = parser.parse_args()

        input_csv = args.input

        master_df = pd.read_csv(input_csv, index_col=False)
        unique_categories = master_df["Element.Category"].unique()
        filtered_dfs = {
            cat: master_df[master_df["Element.Category"] == cat]
            for cat in (unique_categories)
        }

        basicwall_df = process_basicwall(filtered_dfs.get("Walls"))
        roof_df = process_roof(filtered_dfs.get("Gutters"), filtered_dfs.get("Roofs"))
        structural_framing_df = process_structural_framing(filtered_dfs.get("Structural Framing"))
        floors_df = process_floors(filtered_dfs.get("Floors"))
        ceiling_df = process_ceiling(filtered_dfs.get("Ceilings"))


        verified_df = pd.concat(
            [basicwall_df, roof_df, structural_framing_df, floors_df, ceiling_df]
        )

        # Create a df for leftover categories to help user edit lod no matter the result
        non_supported_df = master_df[~master_df.index.isin(verified_df.index)]

        result_df = pd.concat([verified_df,non_supported_df],ignore_index=True)
        result_df.to_csv(args.output, index=False)
        return 0
    except Exception as e:
        print(f"An error occurred: {e}", file=sys.stderr)
        return 1


def create_parser() -> argparse.ArgumentParser:
    parser = argparse.ArgumentParser(
        prog="UniSA LoD Verifyer",
        description="Verify LoD of elements from Naviswokrs model",
    )

    parser.add_argument(
        "input", type=Path, help="Path to Folder of emails or unzipped subfolders"
    )

    parser.add_argument(
        "output",
        type=Path,
        help="Filepath for output result",
    )
    return parser


# Function to check if a value is missing (NaN or empty)
def is_missing(value):
    return pd.isna(value) or str(value).strip() == ""


def assign_lod(
    df: pd.DataFrame, lod_300_properties: list, lod_200_properties: list = None
) -> pd.DataFrame:
    """
    Creates a new DataFrame and assigns Level of Detail (LOD) levels to each row based on the presence
    of certain properties. It also identifies missing properties for each row.

    Args:
        df (pd.DataFrame): The input DataFrame containing the properties for each item.
        lod_300_properties (list): A list of property names required for LOD 300.
        lod_200_properties (list, optional): A list of property names required for LOD 200. Defaults to None.

    Returns:
        pd.DataFrame: A new DataFrame with the LOD levels assigned and missing properties identified.
    """
    # Initialize columns
    df["LOD"] = 100  # Default to LOD 100
    df["Missing_Properties"] = ""

    # Iterate through the DataFrame rows
    for i, row in df.iterrows():
        missing_properties = []
        lod_200_all_present = True

        # Check for LOD 200 properties (if any are provided)
        if lod_200_properties:
            for prop in lod_200_properties:
                if is_missing(row.get(prop)):  # Check if the property is missing
                    missing_properties.append(prop)
                    lod_200_all_present = False

        # If all LOD 200 properties are present, assign LOD 200
        if lod_200_all_present:
            df.at[i, "LOD"] = 200

            # Check for LOD 300 properties (Slope + Gutter)
            lod_300_all_present = True
            for prop in lod_300_properties:
                # Special case: Gutter check (boolean column)
                if prop == "Has_Gutter" and not row.get("Has_Gutter"):
                    missing_properties.append("Gutter")
                    lod_300_all_present = False
                elif prop != "Has_Gutter" and is_missing(row.get(prop)):
                    missing_properties.append(prop)
                    lod_300_all_present = False

            # If all LOD 300 properties are present, assign LOD 300
            if lod_300_all_present:
                df.at[i, "LOD"] = 300

        # Flag missing properties
        df.at[i, "Missing_Properties"] = (
            ", ".join(missing_properties) if missing_properties else ""
        )

    # Add LOD level indicator columns
    df["LOD_100"] = (df["LOD"] == 100).astype(int)
    df["LOD_200"] = (df["LOD"] == 200).astype(int)
    df["LOD_300"] = (df["LOD"] == 300).astype(int)

    return df


def process_roof(gutters_df: pd.DataFrame, roofs_df: pd.DataFrame) -> pd.DataFrame:
    """
    Processes roof and gutter data by cleaning column names, determining whether each roof
    has a corresponding gutter, and assigning the appropriate Level of Detail (LOD).

    Args:
        gutters_df (pd.DataFrame): The DataFrame containing gutter data.
        roofs_df (pd.DataFrame): The DataFrame containing roof data.

    Returns:
        pd.DataFrame: A new DataFrame with LOD levels assigned and a column indicating
                      whether each roof has a corresponding gutter. Returns None if
                      any of the input DataFrames is None.
    """
    # If roofs_df or gutters_df is None, skip this dataset
    if roofs_df is None or gutters_df is None:
        return pd.DataFrame()

    # Check if each roof has a corresponding gutter by matching the Document Title
    roofs_df["Has_Gutter"] = roofs_df["Document.Title"].isin(
        gutters_df["Document.Title"]
    )

    # Define the properties for LOD 200 and LOD 300
    lod_300_properties = [
        "Element.Slope",
        "Has_Gutter",
    ]  # Properties required for LOD 300
    lod_200_properties = ["Element.Thickness"]  # Properties required for LOD 200

    # Run the LOD assignment process and return the modified DataFrame
    return assign_lod(roofs_df, lod_300_properties, lod_200_properties)


def process_basicwall(basicwall_df: pd.DataFrame) -> pd.DataFrame:
    """
    Processes a basic wall DataFrame by cleaning the column names and assigning the
    appropriate Level of Detail (LOD) based on the presence of specific properties.

    Args:
        basicwall_df (pd.DataFrame): The DataFrame containing basic wall data.

    Returns:
        pd.DataFrame: A new DataFrame with LOD levels assigned based on the presence
                      of certain properties.
    """
    if basicwall_df is None:
        return pd.DataFrame()
    # Define the properties for LOD 300 and LOD 200
    lod_300_properties = [
        "Element.Area",
        "Element.Unconnected Height",
        "Element.Length",
    ]  # Properties required for LOD 300

    lod_200_properties = [
        "Revit Type.Width",
        "Revit Type.AUR_MATERIAL TYPE",
        "Item.Material",
    ]  # Properties required for LOD 200

    # Run the LOD assignment process and return the modified DataFrame
    return assign_lod(basicwall_df, lod_300_properties, lod_200_properties)


def process_structural_framing(structural_framing_df: pd.DataFrame) -> pd.DataFrame:
    """
    Processes a structural framing DataFrame by cleaning column names and assigning
    the appropriate Level of Detail (LOD) based on the presence of specific properties.

    Args:
        structural_framing_df (pd.DataFrame): The DataFrame containing structural framing data.

    Returns:
        pd.DataFrame: A new DataFrame with LOD levels assigned based on the presence
                      of certain properties. If the input DataFrame is None, returns
                      an empty DataFrame.
    """
    # If structural_framing_df is None, return an empty DataFrame
    if structural_framing_df is None:
        return pd.DataFrame()

    # Define the properties for LOD 300
    lod_300_properties = [
        "Element.Length",
        "Element.Structural Material",
        "Revit Type.AUR_MATERIAL TYPE",
    ]

    # Run the LOD assignment process and return the modified DataFrame
    return assign_lod(structural_framing_df, lod_300_properties)


def process_floors(floors_df: pd.DataFrame) -> pd.DataFrame:
    """
    Processes a floors DataFrame by cleaning column names and assigning the appropriate
    Level of Detail (LOD) based on the presence of specific properties.

    Args:
        floors_df (pd.DataFrame): The DataFrame containing floors data.

    Returns:
        pd.DataFrame: A new DataFrame with LOD levels assigned based on the presence
                      of certain properties. Returns an empty DataFrame if the input
                      DataFrame is None.
    """
    # If floors_df is None, return an empty DataFrame
    if floors_df is None:
        return pd.DataFrame()

    # Define the properties for LOD 200 and LOD 300
    lod_200_properties = [
        "Element.Area",
    ]  # Properties required for LOD 200
    lod_300_properties = [
        "Element.Elevation at Top",
        "Element.Elevation at Bottom",
        "Element.Thickness",
        "Revit Type.Structural Material",
    ]  # Properties required for LOD 300

    # Run the LOD assignment process and return the modified DataFrame
    return assign_lod(floors_df, lod_300_properties, lod_200_properties)


def process_ceiling(ceilings_df: pd.DataFrame) -> pd.DataFrame:
    """
    Processes a ceiling DataFrame by cleaning column names and assigning the appropriate
    Level of Detail (LOD) based on the presence of specific properties.

    Args:
        ceilings_df (pd.DataFrame): The DataFrame containing ceiling data.

    Returns:
        pd.DataFrame: A new DataFrame with LOD levels assigned based on the presence
                      of certain properties. Returns an empty DataFrame if the input
                      DataFrame is None.
    """
    # If ceilings_df is None, return an empty DataFrame
    if ceilings_df is None:
        return pd.DataFrame()

    # Define the properties for LOD 200 and LOD 300
    lod_200_properties = [
        "Element.Category",
    ]  # Properties required for LOD 200
    lod_300_properties = [
        "Element.Area",
        "Element.Length",
        "Element.Thickness",
    ]  # Properties required for LOD 300

    # Run the LOD assignment process and return the modified DataFrame
    return assign_lod(ceilings_df, lod_300_properties, lod_200_properties)


if __name__ == "__main__":
    main()
    # sys.exit(main())
