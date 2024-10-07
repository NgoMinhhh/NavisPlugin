import pandas as pd
import os


# Function to clean up column names by removing unwanted characters (line breaks, carriage returns, etc.)
def clean_column_names(df):
    df.columns = df.columns.str.replace(r"[\r\n]+", " ", regex=True)  # Remove newlines
    df.columns = df.columns.str.strip()  # Remove leading/trailing spaces
    return df


# Function to check if a value is missing (NaN or empty)
def is_missing(value):
    return pd.isna(value) or str(value).strip() == ""


# Function to assign LOD levels and identify missing properties
def assign_lod(df, lod_300_properties, lod_200_properties=None):
    df["LOD"] = 100  # Default to LOD 100
    df["Missing_Properties"] = ""

    for i, row in df.iterrows():
        missing_properties = []
        lod_200_all_present = True

        # Check for LOD 200 properties (Thickness)
        if lod_200_properties:
            for prop in lod_200_properties:
                if is_missing(row.get(prop)):
                    missing_properties.append(prop)
                    lod_200_all_present = False

        if lod_200_all_present:
            df.at[i, "LOD"] = 200  # If Thickness is present, assign LOD 200

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

            if lod_300_all_present:
                df.at[i, "LOD"] = (
                    300  # If both Slope and Gutter are present, assign LOD 300
                )

        # Flag missing properties
        df.at[i, "Missing_Properties"] = (
            ", ".join(missing_properties) if missing_properties else ""
        )

    # Add the LOD_100, LOD_200, and LOD_300 columns
    df["LOD_100"] = (df["LOD"] == 100).astype(int)
    df["LOD_200"] = (df["LOD"] == 200).astype(int)
    df["LOD_300"] = (df["LOD"] == 300).astype(int)

    return df


# Function to safely load a CSV if it exists
def load_csv_if_exists(file_name):
    if os.path.exists(file_name):
        print(f"Loading {file_name}...")
        return pd.read_csv(file_name)
    else:
        print(f"Warning: {file_name} not found. Skipping this dataset.")
        return None


# Process Roofs
def process_roof():
    # Load the roof and gutter datasets
    gutters_df = load_csv_if_exists("Gutters.csv")
    roofs_df = load_csv_if_exists("Roofs.csv")

    # If roofs_df or gutters_df is None, skip this dataset
    if roofs_df is None or gutters_df is None:
        return None

    # Clean column names (remove unwanted characters)
    gutters_df = clean_column_names(gutters_df)
    roofs_df = clean_column_names(roofs_df)

    # Check if each roof has a corresponding gutter by matching the Document Title
    roofs_df["Has_Gutter"] = roofs_df["Document Title"].isin(
        gutters_df["Document Title"]
    )

    # Define the properties for LOD 200 and LOD 300
    lod_300_properties = [
        "Element Slope",
        "Has_Gutter",
    ]  # Properties required for LOD 300
    lod_200_properties = ["Element Thickness"]  # Properties required for LOD 200

    # Run the LOD assignment process
    return assign_lod(roofs_df, lod_300_properties, lod_200_properties)


# Process each dataset and merge into one final DataFrame


def process_basicwall():
    basicwall_df = load_csv_if_exists("AllBasicWalls.csv")
    if basicwall_df is None:
        return None

    basicwall_df = clean_column_names(basicwall_df)

    lod_300_properties = [
        "Element Area",
        "Element Unconnected Height",
        "Element Length",
        "Element Id",
    ]
    lod_200_properties = [
        "Revit Type Width",
        "Revit Type AUR_MATERIAL TYPE",
        "Item Material",
    ]

    return assign_lod(basicwall_df, lod_300_properties, lod_200_properties)


def process_structural_framing():
    structural_framing_df = load_csv_if_exists("StructualFraming.csv")
    if structural_framing_df is None:
        return None

    structural_framing_df = clean_column_names(structural_framing_df)

    lod_300_properties = [
        "Element Length",
        "Element Structural Material",
        "Element Name",
        "Element Category",
        "Element Family",
        "Revit Type AUR_MATERIAL TYPE",
    ]

    return assign_lod(structural_framing_df, lod_300_properties)


def process_floors():
    floors_df = load_csv_if_exists("Floors.csv")
    if floors_df is None:
        return None

    floors_df = clean_column_names(floors_df)

    lod_200_properties = ["Element Type", "Element Family", "Element Area"]
    lod_300_properties = [
        "Element Elevation at Top",
        "Element Elevation at Bottom",
        "Element Thickness",
        "Revit Type Structural Material",
    ]

    return assign_lod(floors_df, lod_300_properties, lod_200_properties)


def process_ceiling():
    ceilings_df = load_csv_if_exists("Ceilings.csv")
    if ceilings_df is None:
        return None

    ceilings_df = clean_column_names(ceilings_df)

    lod_200_properties = ["Element Category", "Element Family"]
    lod_300_properties = ["Element Area", "Element Length", "Element Thickness"]

    return assign_lod(ceilings_df, lod_300_properties, lod_200_properties)


# Process all datasets and concatenate them into one final DataFrame
def process_all_data():
    basicwall_df = process_basicwall()
    roof_df = process_roof()
    structural_framing_df = process_structural_framing()
    floors_df = process_floors()
    ceiling_df = process_ceiling()

    # Add a new column to differentiate datasets
    final_dataframes = []

    if basicwall_df is not None:
        basicwall_df["Source"] = "BasicWall"
        final_dataframes.append(basicwall_df)
    if roof_df is not None:
        roof_df["Source"] = "Roof"
        final_dataframes.append(roof_df)
    if structural_framing_df is not None:
        structural_framing_df["Source"] = "StructuralFraming"
        final_dataframes.append(structural_framing_df)
    if floors_df is not None:
        floors_df["Source"] = "Floors"
        final_dataframes.append(floors_df)
    if ceiling_df is not None:
        ceiling_df["Source"] = "Ceilings"
        final_dataframes.append(ceiling_df)

    # Concatenate all the DataFrames into one if there are any
    if final_dataframes:
        final_df = pd.concat(final_dataframes, ignore_index=True)
        # Save the final DataFrame into one CSV file
        final_df.to_csv("Final_LOD_Output_with_LOD_columns.csv", index=False)
        print(final_df)
    else:
        print("No data processed as no CSV files were found.")


# Run the process
process_all_data()
