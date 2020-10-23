Imports System.Data.SqlClient

Public Class Form1
    Dim newewewewDT As New DataTable

    Private Sub Form1_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Timer1.Interval = 10000
        'Timer1.Enabled = True
        newewewewDT.Columns.Add("Type")
        newewewewDT.Columns.Add("TimeEx")

        ClockInTransfer()

    End Sub


    Private Sub BackgroundWorker1_DoWork(sender As Object, e As System.ComponentModel.DoWorkEventArgs) Handles BackgroundWorker1.DoWork
        'THIS IS WHERE YOU PLACE FUNCTIONS

        'Dim FillCheck = 0
        'Dim TCheck = 0


        'If FillCheck = 0 Then
        '    FillUsers()
        '    'newewewewDT.Rows.Add("Fill Users Completed", DateTime.Now)
        '    FillCheck = 1
        'End If


        'If FillCheck = 1 And TCheck = 0 Then
        '    TransferQueriesToHome()
        '    'newewewewDT.Rows.Add("Transfer Queries Complete", DateTime.Now)
        'End If


        'ClockInTransfer()



    End Sub



    Private Sub BackgroundWorker1_ProgressChanged(sender As Object, e As System.ComponentModel.ProgressChangedEventArgs) Handles BackgroundWorker1.ProgressChanged



    End Sub



    Private Sub BackgroundWorker1_RunWorkerCompleted(sender As Object, e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles BackgroundWorker1.RunWorkerCompleted



    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        ' BackgroundWorker1.WorkerSupportsCancellation = True
        ' BackgroundWorker1.WorkerReportsProgress = True
        BackgroundWorker1.RunWorkerAsync()
        DataGridView2.DataSource = newewewewDT


    End Sub


    Public Sub ClockInTransfer()
        Dim connStr As String = Connection()
        Dim WS As New EmployeePortalWS.WebService

        Dim poo = 1


        ' WS.ResetClockInBoolean()
        Do While poo = 1

            Dim NoOfClockins = WS.ReturnClockinCheck

            If NoOfClockins > 0 Then

                Dim dt As DataTable = WS.Get_Clockins_Home_Server

                'Dim sjhs As Boolean = WS.ReturnClockinCheck
                Using conn As New SqlConnection(connStr)
                    conn.Open()

                    For i As Integer = 0 To dt.Rows.Count - 1

                        Dim dtTemp As DataTable = GetDataTable(" Select Top(1)  EMP_CLOCKINGS.EmployeeName, EMP_CLOCKINGS.ID, EMP_CLOCKINGS.ClockingType, EMP_CLOCKINGS.Clocking_Date, EMP_CLOCKINGS.TimeInForHourAndMinutes " &
                                             " From Users INNER Join " &
                                             "              EMP_CLOCKINGS On EMP_CLOCKINGS.EmployeeName = Users.User_Name " &
                                             " Where (EMP_CLOCKINGS.Clocking_Date BETWEEN DATEADD(day, DATEDIFF(day, 0, GETDATE()), 0) + '03:00' AND DATEADD(day, DATEDIFF(day, - 1, GETDATE()), 0) + '02:59') AND (EMP_CLOCKINGS.EmployeeName = '" & dt.Rows(i).Item(8).ToString() & "') " &
                                             " Order By EMP_CLOCKINGS.Clocking_Date DESC")

                        Dim ClockInTypeInsert = dt.Rows(i).Item(3).ToString()

                        'ADD PART TO IGNORE IF NO ENTRY
                        Dim LastClockInAtHomeServer = dtTemp.Rows(0)("ClockingType").ToString()



                        If ClockInTypeInsert = LastClockInAtHomeServer Then

                            Select Case ClockInTypeInsert
                                Case "IN"

                                    WS.SendQueryToDb123("UPDATE [dbo].[EPortal_EMP_CLOCKINGS] SET [TransferError] = 1, [TransferErrorMessage] = 'Duplicate Clocking Type IN', [TransferErrorTime] =  GETDATE() where ID = " & dt.Rows(i).Item(0) & "", False)

                                Case "OUT"

                                    WS.SendQueryToDb123("UPDATE [dbo].[EPortal_EMP_CLOCKINGS] SET [TransferError] = 1, [TransferErrorMessage] = 'Duplicate Clocking Type OUT', [TransferErrorTime] =  GETDATE() where ID = " & dt.Rows(i).Item(0) & "", False)

                            End Select

                        Else
                            Dim HomeServerId As String
                            Using cmd As New SqlCommand()

                                With cmd

                                    Select Case ClockInTypeInsert

                                        Case "IN"

                                            .Connection = conn
                                            .CommandText = ("INSERT INTO [dbo].[EMP_CLOCKINGS]" &
                                                            " ([ClockCardNumber], [Clocking_Date], [ClockingType], [TimeInForSeconds], [TimeInForMinutes], [TimeInForHours], [TimeInForHourAndMinutes], [EmployeeName] ,[SiteName] ,[ComputerName], [OriginalTimeIn] , [RelatedClocking] )" &
                                                            " VALUES" &
                                                            " (@Param1, @Param2, @Param3, @Param4, @Param5, @Param6, @Param7, @Param8, @Param9, @Param10, @Param11, @Param12) SELECT SCOPE_IDENTITY();")
                                            .CommandType = CommandType.Text
                                            .Parameters.Add("@Param1", SqlDbType.VarChar).Value = dt.Rows(i).Item(1).ToString()
                                            .Parameters.Add("@Param2", SqlDbType.DateTime).Value = dt.Rows(i).Item(2)
                                            .Parameters.Add("@Param3", SqlDbType.VarChar).Value = dt.Rows(i).Item(3).ToString()
                                            .Parameters.Add("@Param4", SqlDbType.VarChar).Value = dt.Rows(i).Item(4).ToString()
                                            .Parameters.Add("@Param5", SqlDbType.VarChar).Value = dt.Rows(i).Item(5).ToString()
                                            .Parameters.Add("@Param6", SqlDbType.VarChar).Value = dt.Rows(i).Item(6).ToString()
                                            .Parameters.Add("@Param7", SqlDbType.VarChar).Value = dt.Rows(i).Item(7).ToString()
                                            .Parameters.Add("@Param8", SqlDbType.VarChar).Value = dt.Rows(i).Item(8).ToString()
                                            .Parameters.Add("@Param9", SqlDbType.VarChar).Value = dt.Rows(i).Item(9).ToString()
                                            .Parameters.Add("@Param10", SqlDbType.VarChar).Value = dt.Rows(i).Item(10).ToString()
                                            .Parameters.Add("@Param11", SqlDbType.DateTime).Value = dt.Rows(i).Item(11)
                                            .Parameters.Add("@Param12", SqlDbType.VarChar).Value = dt.Rows(i).Item(12).ToString()

                                        Case "OUT"

                                            .Connection = conn
                                            .CommandText = ("INSERT INTO [dbo].[EMP_CLOCKINGS]" &
                                                            " ([ClockCardNumber], [Clocking_Date], [ClockingType], [TimeInForSeconds], [TimeInForMinutes], [TimeInForHours], [TimeInForHourAndMinutes], [EmployeeName] ,[SiteName] ,[ComputerName], [OriginalTimeIn] , [RelatedClocking] )" &
                                                            " VALUES" &
                                                            " (@Param1, @Param2, @Param3, @Param4, @Param5, @Param6, @Param7, @Param8, @Param9, @Param10, @Param11, @Param12) SELECT SCOPE_IDENTITY();")
                                            .CommandType = CommandType.Text
                                            .Parameters.Add("@Param1", SqlDbType.VarChar).Value = dt.Rows(i).Item(1).ToString()
                                            .Parameters.Add("@Param2", SqlDbType.DateTime).Value = dt.Rows(i).Item(2)
                                            .Parameters.Add("@Param3", SqlDbType.VarChar).Value = dt.Rows(i).Item(3).ToString()
                                            .Parameters.Add("@Param4", SqlDbType.VarChar).Value = dt.Rows(i).Item(4).ToString()
                                            .Parameters.Add("@Param5", SqlDbType.VarChar).Value = dt.Rows(i).Item(5).ToString()
                                            .Parameters.Add("@Param6", SqlDbType.VarChar).Value = dt.Rows(i).Item(6).ToString()
                                            .Parameters.Add("@Param7", SqlDbType.VarChar).Value = dt.Rows(i).Item(7).ToString()
                                            .Parameters.Add("@Param8", SqlDbType.VarChar).Value = dt.Rows(i).Item(8).ToString()
                                            .Parameters.Add("@Param9", SqlDbType.VarChar).Value = dt.Rows(i).Item(9).ToString()
                                            .Parameters.Add("@Param10", SqlDbType.VarChar).Value = dt.Rows(i).Item(10).ToString()
                                            .Parameters.Add("@Param11", SqlDbType.DateTime).Value = dt.Rows(i).Item(11)
                                            .Parameters.Add("@Param12", SqlDbType.VarChar).Value = dtTemp.Rows(0).Item(1).ToString()
                                    End Select


                                End With

                                HomeServerId = cmd.ExecuteScalar()


                            End Using

                            WS.SendQueryToDb123("UPDATE [dbo].[EPortal_EMP_CLOCKINGS] SET [TransferCheck] = 1, [HomeServerID] = " & HomeServerId & " where ID = " & dt.Rows(i).Item(0) & "", False)
                            WS.SendQueryToDb123("INSERT INTO [dbo].[EPortal_TransferLog] (LogType, LogTime) VALUES ('Clocking Insert to Home DB', GETDATE())", False)

                        End If

                    Next

                    conn.Close()

                End Using


                'WS.ResetClockInBoolean()

            End If


        Loop

    End Sub




    Sub TransferQueriesToHome()

        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable = WS.Get_SQL_Queries_For_Home_Server()

        'DataGridView1.DataSource = dt

        RunSQL(dt)

    End Sub


    Private Sub TransferQueries_Click(sender As Object, e As EventArgs) Handles TransferQueries.Click

        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable = WS.Get_SQL_Queries_For_Home_Server()

        'DataGridView1.DataSource = dt

        RunSQL(dt)

    End Sub


    Sub RunSQL(ByRef dt As DataTable)

        Dim WS As New EmployeePortalWS.WebService

        For Each row In dt.Rows


            Dim TransferSection = row.itemArray(0)

            If Not TransferSection = "" Or TransferSection IsNot Nothing Then

                'Try

                Select Case TransferSection
                        Case "Account Update"
                            RunAccountUpdates(row.itemArray(1))
                        WS.SendQueryToDb123("INSERT INTO [dbo].[EPortal_TransferLog] (LogType, LogTime) VALUES ('User account updates sent to Home DB', GETDATE())", False)
                    Case "Holiday Request"
                            InsertHolidayRequest(row.itemArray(1))
                        WS.SendQueryToDb123("INSERT INTO [dbo].[EPortal_TransferLog] (LogType, LogTime) VALUES ('Holidays inserted to Home DB', GETDATE())", False)
                    Case "Appointment Request"
                            InsertAppointmentRequest(row.itemArray(1))
                        WS.SendQueryToDb123("INSERT INTO [dbo].[EPortal_TransferLog] (LogType, LogTime) VALUES ('Appointments inserted to Home DB', GETDATE())", False)
                    Case "Cancel Holiday"
                            CancelHolidayRequest(row.itemArray(1))
                        WS.SendQueryToDb123("INSERT INTO [dbo].[EPortal_TransferLog] (LogType, LogTime) VALUES ('Cancelled Holidays sent to Home DB', GETDATE())", False)
                End Select

                'Catch ex As SqlException
                ' MsgBox("Error: " & vbCrLf & ex.Message)

                'End Try

            Else

                'MsgBox("Nothing to Transfer!")

            End If

        Next

    End Sub


    Public Sub RunAccountUpdates(ByRef QueryCount As Integer)

        Dim connStr As String = Connection()
        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable = WS.GetDataTableWS("SELECT [SqlScript], [ID] FROM [dbo].[EPortal_DataTransfer] WHERE TransferCheck = 0")

        Using conn As New SqlConnection(connStr)
            conn.Open()

            If dt.Rows.Count = QueryCount Then
                For i As Integer = 0 To dt.Rows.Count - 1

                    Dim ID = dt.Rows(i).Item(1).ToString()

                    Using cmd As New SqlCommand()

                        With cmd
                            .Connection = conn
                            .CommandText = (dt.Rows(i).Item(0).ToString())
                            .CommandType = CommandType.Text
                        End With

                        cmd.ExecuteScalar()

                        WS.SendQueryToDb123("UPDATE [dbo].[EPortal_DataTransfer] SET [TransferCheck] = 1 WHERE ID = " & ID & "", False)

                    End Using

                Next
            End If

            conn.Close()

        End Using

    End Sub

    Public Sub InsertHolidayRequest(ByRef QueryCount As Integer)

        Dim connStr As String = Connection()
        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable = WS.GetDataTableWS("SELECT * FROM [dbo].[HR_ParameterisedQueries] WHERE QueryName = 'Holiday Transfer'")

        Using conn As New SqlConnection(connStr)
            conn.Open()

            If dt.Rows.Count = QueryCount Then
                For i As Integer = 0 To dt.Rows.Count - 1


                    Using cmd As New SqlCommand()

                        With cmd


                            .Connection = conn
                            .CommandText = ("INSERT INTO [dbo].[HR_HolidayRequests]" &
                                            " ([EmployeeName], [PayrollNumber], [HolidayType], [StartDate], [EndDate], [HolidayDate], [NumberOfHolidayWorkingDays], [DateRequested])" &
                                            " VALUES" &
                                            " (@Param1, @Param2, @Param3, @Param4, @Param5, @Param6, @Param7, @Param8)")
                            .CommandType = CommandType.Text
                            .Parameters.Add("@Param1", SqlDbType.VarChar).Value = dt.Rows(i).Item(2).ToString()
                            .Parameters.Add("@Param2", SqlDbType.VarChar).Value = dt.Rows(i).Item(3).ToString()
                            .Parameters.Add("@Param3", SqlDbType.VarChar).Value = dt.Rows(i).Item(4).ToString()
                            .Parameters.Add("@Param4", SqlDbType.Date).Value = Date.ParseExact(dt.Rows(i).ItemArray(5), "MM/d/yyyy", Nothing)
                            .Parameters.Add("@Param5", SqlDbType.Date).Value = Date.ParseExact(dt.Rows(i).Item(6), "MM/d/yyyy", Nothing)
                            .Parameters.Add("@Param6", SqlDbType.Date).Value = Date.ParseExact(dt.Rows(i).Item(7), "MM/d/yyyy", Nothing)
                            .Parameters.Add("@Param7", SqlDbType.VarChar).Value = dt.Rows(i).Item(8).ToString()
                            .Parameters.Add("@Param8", SqlDbType.DateTime).Value = DateTime.ParseExact(dt.Rows(i).Item(9), "MM/dd/yyyy h:mm:ss tt", Nothing)
                        End With



                        cmd.ExecuteScalar()

                        WS.SendQueryToDb123("UPDATE [dbo].[HR_HolidayRequests_PendingApproval] SET [TransferCheck] = 1 " &
                                            " WHERE EmployeeName = '" & dt.Rows(i).Item(2).ToString() & "' AND HolidayDate = '" & dt.Rows(i).Item(7).ToString() & "' AND DateRequested = '" & dt.Rows(i).Item(9).ToString() & "'", False)




                    End Using

                Next
            End If

            conn.Close()

        End Using

    End Sub


    Public Sub InsertAppointmentRequest(ByRef QueryCount As Integer)

        Dim connStr As String = Connection()
        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable = WS.GetDataTableWS("SELECT * FROM [dbo].[HR_ParameterisedQueries] WHERE QueryName = 'Appointment Transfer'")

        Using conn As New SqlConnection(connStr)
            conn.Open()

            If dt.Rows.Count = QueryCount Then
                For i As Integer = 0 To dt.Rows.Count - 1

                    Using cmd As New SqlCommand()

                        With cmd
                            .Connection = conn
                            .CommandText = ("INSERT INTO [dbo].[HR_EmployeeSickness] ([EmployeeName], [SicknessType], [DateOfSickness], [EnteredBy], [DateEntered], [Notes]) VALUES (@Param1, @Param2, @Param3, @Param4, @Param5, @Param6)")
                            .CommandType = CommandType.Text
                            .Parameters.Add("@Param1", SqlDbType.VarChar).Value = dt.Rows(i).Item(2).ToString()
                            .Parameters.Add("@Param2", SqlDbType.VarChar).Value = dt.Rows(i).Item(3).ToString()
                            .Parameters.Add("@Param3", SqlDbType.Date).Value = Date.ParseExact(dt.Rows(i).Item(4), "MM/d/yyyy", Nothing)
                            .Parameters.Add("@Param4", SqlDbType.VarChar).Value = dt.Rows(i).Item(5).ToString()
                            .Parameters.Add("@Param5", SqlDbType.DateTime).Value = Date.ParseExact(dt.Rows(i).Item(6), "MM/d/yyyy", Nothing)
                            .Parameters.Add("@Param6", SqlDbType.VarChar).Value = dt.Rows(i).Item(7).ToString()
                        End With

                        cmd.ExecuteScalar()

                        WS.SendQueryToDb123("UPDATE [dbo].[HR_EmployeeSickness_PendingApproval] SET [TransferCheck] = 1 WHERE EmployeeName = '" & dt.Rows(i).Item(2).ToString() & "' AND DateOfSickness = '" & dt.Rows(i).Item(4).ToString() & "' AND DateEntered = '" & dt.Rows(i).Item(6).ToString() & "'", False)

                    End Using

                Next
            End If

            conn.Close()

        End Using

    End Sub

    Public Sub CancelHolidayRequest(ByRef QueryCount As Integer)

        Dim connStr As String = Connection()
        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable = WS.GetDataTableWS("SELECT * FROM [dbo].[HR_ParameterisedQueries] WHERE QueryName = 'Cancel Holiday'")

        Using conn As New SqlConnection(connStr)
            conn.Open()

            If dt.Rows.Count = QueryCount Then
                For i As Integer = 0 To dt.Rows.Count - 1

                    Using cmd As New SqlCommand()


                        With cmd
                            .Connection = conn
                            .CommandText = ("UPDATE [dbo].[HR_HolidayRequests] SET [Cancelled] = 1 WHERE EmployeeName = @Param1 AND HolidayDate = @Param2")
                            .CommandType = CommandType.Text
                            .Parameters.Add("@Param1", SqlDbType.VarChar).Value = dt.Rows(i).Item(2).ToString()
                            .Parameters.Add("@Param2", SqlDbType.Date).Value = Date.ParseExact(dt.Rows(i).Item(3), "MM/d/yyyy", Nothing)

                        End With

                        cmd.ExecuteScalar()

                        WS.SendQueryToDb123("UPDATE [dbo].[HR_HolidayRequests_PendingApproval] SET [CancelCheck] = 1 WHERE EmployeeName = '" & dt.Rows(i).Item(2).ToString() & "' AND HolidayDate = '" & dt.Rows(i).Item(3).ToString() & "'", False)

                    End Using

                Next
            End If

            conn.Close()

        End Using

    End Sub


    Private Function Connection() As String

        Connection = "Data Source = ZOOSTORM2 \ SQLEXPRESS;Initial Catalog=FOURDSBLINDS;Persist Security Info=True;User ID=sa;Password=Welcome2Bloc"

        Return Connection
    End Function


    Public Function FunGetSQL(stringSQL As String) As String

        Dim connStr As String = Connection()
        Dim retVal As String

        Using conn As New SqlConnection(connStr)
            conn.Open()

            Using cmd As New SqlCommand()
                With cmd
                    .Connection = conn
                    .CommandText = (stringSQL)
                    .CommandType = CommandType.Text
                End With

                retVal = cmd.ExecuteScalar()

            End Using

            conn.Close()

        End Using

        Return retVal
    End Function

    Public Sub SubRunSQL(ByRef stringSQL As String)

        Dim connStr As String = Connection()

        Using conn As New SqlConnection(connStr)
            conn.Open()

            Using cmd As New SqlCommand()
                With cmd
                    .Connection = conn
                    .CommandText = (stringSQL)
                    .CommandType = CommandType.Text
                End With

                cmd.ExecuteScalar()
            End Using

            conn.Close()

        End Using
    End Sub


    Public Function GetDataTable(stringSQL As String) As DataTable
        'the DataTable to return
        Dim MyDataTable As New DataTable

        Dim connStr As String = Connection()

        'make a SqlConnection using the supplied ConnectionString 
        Using conn As New SqlConnection(connStr)
            conn.Open()
            'make a query using the supplied Sql
            Dim cmd As SqlCommand = New SqlCommand(stringSQL, conn)

            'create a DataReader and execute the SqlCommand
            Dim MyDataReader As SqlDataReader = cmd.ExecuteReader()

            'load the reader into the datatable
            MyDataTable.Load(MyDataReader)

            'clean up
            MyDataReader.Close()
            conn.Close()
        End Using

        'return the datatable
        Return MyDataTable

    End Function


    Private Sub FillUsers()
        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable
        dt = GetDataTable("SELECT * FROM  [dbo].[Users] ")
        dt.TableName = "blah"

        WS.CreateSQLTableFromDataTable(dt, "Users2", True)
    End Sub

    Private Sub BtnFillUsers_Click(sender As Object, e As EventArgs) Handles BtnUpdateUsers.Click

        Dim WS As New EmployeePortalWS.WebService

        Dim dt As DataTable
        dt = GetDataTable("SELECT * FROM  [dbo].[Users] ")
        dt.TableName = "blah"

        WS.CreateSQLTableFromDataTable(dt, "Users2", True)

    End Sub

End Class
