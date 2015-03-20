Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Text

Imports System.Data.SQLite
Imports System.Data


Public Class DataBaseManager

    Private dbConnection As String
 
    ' <summary>
    'Default Constructor for SQLiteDatabase Class.
    '</summary>
    Public Sub DataBaseManager()
        dbConnection = DataBaseConnection()
    End Sub


    Public Function DataBaseConnection() As String
        Dim dataBasePath As String = New Uri(System.IO.Path.GetDirectoryName( _
      System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase)).LocalPath
        DataBaseConnection = "Data Source=" + dataBasePath + "\categoriesDatabase.db" + ";Version=3"
    End Function

    Public Sub DeleteTable(level As Integer)
        DataBaseConnection()
        Dim SQLConnect As SQLiteConnection = New SQLiteConnection(DataBaseConnection)
        If SQLConnect.State <> ConnectionState.Open Then
            SQLConnect.Open()
        End If
        Dim SQLCommand As SQLiteCommand = SQLConnect.CreateCommand()
        Dim SQLDelete As String = "DELETE FROM Categories_level_"
        SQLDelete += CStr(level)
        SQLCommand.CommandText = SQLDelete
        SQLCommand.ExecuteNonQuery()
        SQLConnect.Close()
    End Sub

    Public Sub UpdateDataBase(categoryArray As ebay.CategoryType())
        DataBaseConnection()
        Dim SQLConnect As SQLiteConnection = New SQLiteConnection(DataBaseConnection)
        Dim SQLCommand As SQLiteCommand = SQLConnect.CreateCommand()
        Dim levelIndex As String
        Dim categoryID As String
        Dim categoryLevel As String
        Dim bestOfferEnabled As String
        Dim categoryParentID As String
        Dim categoryName As String

        For level As Integer = 1 To 6
            DeleteTable(level)

        Next
        If SQLConnect.State <> ConnectionState.Open Then
            SQLConnect.Open()
        End If
        'Start A unique Transaction
        SQLCommand = New SQLiteCommand("begin", SQLConnect)
        SQLCommand.ExecuteNonQuery()
        For Each categoryItem As ebay.CategoryType In categoryArray

            levelIndex = CStr(categoryItem.CategoryLevel)
            categoryID = CStr(categoryItem.CategoryID)
            categoryLevel = CStr(categoryItem.CategoryLevel)
            bestOfferEnabled = CStr(categoryItem.BestOfferEnabled)
            categoryParentID = CStr(categoryItem.CategoryParentID(0))
            categoryName = CStr(categoryItem.CategoryName)

            SQLCommand.CommandText = "INSERT INTO Categories_level_" + _
                                   levelIndex + _
                                   "(CategoryID, CategoryName, CategoryLevel, BestOfferEnabled, CategoryParentID) VALUES (?,?,?,?,?)"
            SQLCommand.Parameters.AddWithValue("CategoryID", categoryID)
            SQLCommand.Parameters.AddWithValue("CategoryName", categoryName)
            SQLCommand.Parameters.AddWithValue("CategoryLevel", categoryLevel)
            SQLCommand.Parameters.AddWithValue("BestOfferEnabled", bestOfferEnabled)
            SQLCommand.Parameters.AddWithValue("CategoryParentID", categoryParentID)


            'Execute the Update Statement
            SQLCommand.ExecuteNonQuery()
        Next
        'Finish The Data Transaction
        SQLCommand = New SQLiteCommand("end", SQLConnect)
        SQLCommand.ExecuteNonQuery()



        'Close The Connection
        SQLConnect.Close()
        Console.WriteLine("The DataBase has Been Regenerated.")

    End Sub

    Sub UpdateDabaseInfo(dateInfo As String, versionInfo As String, sourceInfo As String)
        DataBaseConnection()
        Dim SQLConnect As SQLiteConnection = New SQLiteConnection(DataBaseConnection)

        If SQLConnect.State <> ConnectionState.Open Then
            SQLConnect.Open()
        End If
        ' Delete Previous DataBase Info
        Dim SQLDelete As String = "DELETE FROM DataBaseInfo"
        Dim SQLCommand As SQLiteCommand = SQLConnect.CreateCommand()
        SQLCommand = New SQLiteCommand(SQLDelete, SQLConnect)
        SQLCommand.ExecuteNonQuery()

        SQLCommand = New SQLiteCommand
        SQLCommand.CommandText = "INSERT INTO DataBaseInfo (DateOfCreation, Version, DataSource) VALUES (?,?,?)"
        SQLCommand.Parameters.AddWithValue("DateOfCreation", dateInfo)
        SQLCommand.Parameters.AddWithValue("Version", versionInfo)
        SQLCommand.Parameters.AddWithValue("DataSource", sourceInfo)
        ' Write New DataBase information
        SQLCommand.Connection = SQLConnect
        SQLCommand.ExecuteNonQuery()

        SQLConnect.Close()
    End Sub

    Function findCategoryLevelByID(categoryID As Integer) As Integer
        Dim returnValue As Integer = 0
        Dim dataTable As DataTable = New DataTable()
        Dim SQLConnect As SQLiteConnection = New SQLiteConnection(DataBaseConnection)
        Dim dataAdapter As SQLiteDataAdapter = New SQLiteDataAdapter()
        If SQLConnect.State <> ConnectionState.Open Then
            SQLConnect.Open()
        End If
        Dim SQLCommand = New SQLiteCommand
        SQLCommand.Connection = SQLConnect

        Dim SQLSelect1 As String = "SELECT * FROM Categories_level_"
        Dim SQLSelect2 As String = " WHERE CategoryID = ?"

        For level As Integer = 1 To 6
            SQLCommand.CommandText = SQLSelect1 + CStr(level) + SQLSelect2
            SQLCommand.Parameters.AddWithValue("CategoryID", categoryID)
            dataAdapter.SelectCommand = SQLCommand
            dataAdapter.Fill(dataTable)

            If dataTable.Rows.Count > 0 Then
                returnValue = level
                Exit For
            End If
        Next

        findCategoryLevelByID = returnValue
    End Function

    Function ReturnAllChildsByID(startLevel As Integer, referenceID As Integer) As List(Of DataRow)
        Dim listOfChilds As New List(Of DataRow)

        Dim dataTable As DataTable = New DataTable()
        Dim SQLConnect As SQLiteConnection = New SQLiteConnection(DataBaseConnection)
        Dim dataAdapter As SQLiteDataAdapter = New SQLiteDataAdapter()
        If SQLConnect.State <> ConnectionState.Open Then
            SQLConnect.Open()
        End If
        Dim SQLCommand = New SQLiteCommand
        SQLCommand.Connection = SQLConnect

        Dim SQLSelect1 As String = "SELECT * FROM Categories_level_"
        Dim SQLSelect2 As String = " WHERE CategoryParentID = ?"
        SQLCommand.CommandText = SQLSelect1 + CStr(startLevel + 1) + SQLSelect2
        SQLCommand.Parameters.AddWithValue("CategoryParentID", referenceID)
        dataAdapter.SelectCommand = SQLCommand
        dataAdapter.Fill(dataTable)
        If (dataTable.Rows.Count <> 0) Then
            For index As Integer = 0 To dataTable.Rows.Count - 1
                listOfChilds.Add(dataTable.Rows(index))
            Next
        End If

        ReturnAllChildsByID = listOfChilds
    End Function

    Function ReturnDataByID(startLevel As Integer, referenceID As Integer) As DataRow



        Dim SQLConnect As SQLiteConnection = New SQLiteConnection(DataBaseConnection)
        Dim dataTable As DataTable = New DataTable()
        Dim dataAdapter As SQLiteDataAdapter = New SQLiteDataAdapter()
        If SQLConnect.State <> ConnectionState.Open Then
            SQLConnect.Open()
        End If
        Dim SQLCommand = New SQLiteCommand
        SQLCommand.Connection = SQLConnect

        Dim SQLSelect1 As String = "SELECT * FROM Categories_level_"
        Dim SQLSelect2 As String = " WHERE CategoryID = ?"
        SQLCommand.CommandText = SQLSelect1 + CStr(startLevel) + SQLSelect2
        SQLCommand.Parameters.AddWithValue("CategoryID", referenceID)
        dataAdapter.SelectCommand = SQLCommand
        dataAdapter.Fill(dataTable)


        ReturnDataByID = dataTable.Rows(0)


    End Function

    Function ReturnDataBaseData() As String



        Dim SQLConnect As SQLiteConnection = New SQLiteConnection(DataBaseConnection)
        Dim dataTable As DataTable = New DataTable()
        Dim dataAdapter As SQLiteDataAdapter = New SQLiteDataAdapter()
        If SQLConnect.State <> ConnectionState.Open Then
            SQLConnect.Open()
        End If
        Dim SQLCommand = New SQLiteCommand
        SQLCommand.Connection = SQLConnect

        Dim SQLSelect As String = "SELECT * FROM DataBaseInfo"

        SQLCommand.CommandText = SQLSelect
        dataAdapter.SelectCommand = SQLCommand
        dataAdapter.Fill(dataTable)


        ReturnDataBaseData = dataTable.Rows(0).Item(0).ToString


    End Function

End Class
