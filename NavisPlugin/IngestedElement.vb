''' <summary>
''' Represents an element ingested from the CSV file, containing only necessary properties.
''' </summary>
Public Class IngestedElement
    ' Custom class to store only necessary info from CSV

    ''' <summary>
    ''' Gets or sets the GUID of the element.
    ''' </summary>
    Public Property GUID As String

    ''' <summary>
    ''' Gets or sets the Level of Detail (LOD) of the element.
    ''' </summary>
    Public Property LOD As String

    ''' <summary>
    ''' Gets or sets the missing properties of the element.
    ''' </summary>
    Public Property MissingProperties As String

    ''' <summary>
    ''' Gets or sets the search result status of the element.
    ''' </summary>
    Public Property SearchResult As String

    ''' <summary>
    ''' Gets or sets the source model of the element.
    ''' </summary>
    Public Property Source As String


    ''' <summary>
    ''' Initializes a new instance of the IngestedElement class with default SearchResult.
    ''' </summary>
    Public Sub New()
        ' Constructor to initialize SearchResult with a default value
        SearchResult = "Not Search Yet"
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the IngestedElement class with specified properties.
    ''' </summary>
    ''' <param name="guid">The GUID of the element.</param>
    ''' <param name="lod">The Level of Detail of the element.</param>
    ''' <param name="missingProps">The missing properties of the element.</param>
    Public Sub New(guid As String, lod As String, missingProps As String, source As String)
        Me.GUID = guid
        Me.LOD = lod
        MissingProperties = missingProps
        source = source
        SearchResult = "Not Search Yet" ' Default value
    End Sub

    ''' <summary>
    ''' Initializes a new instance of the IngestedElement class by copying an existing instance and optionally setting a new SearchResult.
    ''' </summary>
    ''' <param name="existingElement">The existing IngestedElement to copy.</param>
    ''' <param name="newSearchResult">The new search result status. Defaults to "Not Search Yet".</param>
    Public Sub New(existingElement As IngestedElement, Optional newSearchResult As String = "Not Search Yet")
        ' Copy constructor with option to set a new SearchResult
        GUID = existingElement.GUID
        LOD = existingElement.LOD
        MissingProperties = existingElement.MissingProperties
        Source = existingElement.Source
        SearchResult = newSearchResult
    End Sub

    ''' <summary>
    ''' Returns a string representation of the IngestedElement.
    ''' </summary>
    ''' <returns>A string containing the GUID, LOD, Missing Properties, and Search Result.</returns>
    Public Overrides Function ToString() As String
        Return $"GUID: {GUID}, LOD: {LOD}, Missing Properties: {MissingProperties}, Source: {Source},Search Result: {SearchResult}"
    End Function
End Class