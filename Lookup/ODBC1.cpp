#include <windows.h>
#include "stdafx.h"
#include <sql.h>
#include <sqlext.h>
#include <sqltypes.h>
#include <iostream>
#include <conio.h>

#pragma comment(lib, "odbc32.lib")
#pragma comment(lib, "user32.lib")

using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	#define MAXBUFLEN   255

	SQLHENV hEnv = SQL_NULL_HENV;
	SQLHDBC hDBC = SQL_NULL_HDBC;
	SQLHSTMT hStmt = SQL_NULL_HSTMT;
	SQLCHAR ConnStrIn[MAXBUFLEN] = "DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=Northwind;Trusted_Connection=yes";
	SQLCHAR ConnStrOut[MAXBUFLEN];
	SQLSMALLINT cbConnStrOut = 0;

	// Allocate environment handle
	SQLRETURN retcode = SQLAllocHandle( SQL_HANDLE_ENV, SQL_NULL_HANDLE, &hEnv );

	// Set the ODBC version environment attribute
	if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
		retcode = SQLSetEnvAttr( hEnv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER*)SQL_OV_ODBC3, 0 );

		// Allocate connection handle
		if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
			retcode = SQLAllocHandle( SQL_HANDLE_DBC, hEnv, &hDBC );

			// Set login timeout to 3 seconds
			if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
				SQLSetConnectAttr(hDBC, SQL_LOGIN_TIMEOUT, (SQLPOINTER)3, 0);

				//HWND desktopHandle = GetDesktopWindow();   // desktop's window handle
				//retcode = SQLDriverConnect( hDBC, desktopHandle, ConnStrIn, SQL_NTS, ConnStrOut, MAXBUFLEN, &cbConnStrOut, SQL_DRIVER_PROMPT );
				retcode = SQLDriverConnect( hDBC, NULL, ConnStrIn, SQL_NTS, ConnStrOut, MAXBUFLEN, &cbConnStrOut, SQL_DRIVER_COMPLETE );
				//if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
				if (SQL_SUCCEEDED(retcode))
				{
					//cout << "Connected to database " << endl
					//	<< "Connection Info: " << endl
					//	<< ConnStrOut << endl;


					LARGE_INTEGER begin, end, freq;
					QueryPerformanceCounter(&begin);

					// Allocate statement handle
					retcode = SQLAllocHandle( SQL_HANDLE_STMT,	hDBC, &hStmt );
					if (retcode == SQL_SUCCESS || retcode == SQL_SUCCESS_WITH_INFO) {
						retcode = SQLExecDirect(hStmt, (SQLCHAR*)"SELECT FirstName FROM Employees;", SQL_NTS);

						SQLSMALLINT nCols = 0;
						SQLINTEGER	nRows = 0;
						//SQLINTEGER	nIdicator = 0;
						SQLCHAR		buf[1024];
						BYTE		pByte;
						PBYTE		pPicture;
						SQLINTEGER  pIndicators[2];

						SQLNumResultCols(hStmt, &nCols);
						SQLRowCount(hStmt, &nRows);
						while(SQLFetch( hStmt ) == SQL_SUCCESS)
						{
							//cout << "Row " << endl;
							for( int i = 1; i <= nCols; ++i )
							{
								if (i != 15) {
									retcode = SQLGetData(hStmt,	i, SQL_C_CHAR, buf, 1024, &pIndicators[0] );
									if(SQL_SUCCEEDED(retcode))
									{
										//cout << buf << endl;
										//cout << "Column " << buf << endl;
									}
								}
								else
								{
									// Call SQLGetData to determine the amount of data that's waiting.
									//pPicture = new BYTE[1];
									if (SQLGetData(hStmt, 15, SQL_C_BINARY, (SQLPOINTER)&pByte, 0, &pIndicators[1]) == SQL_SUCCESS_WITH_INFO)
									{
										cout << "Photo size: " << pIndicators[1] << "\n\n";

										// Get all the data at once.
										pPicture = new BYTE[pIndicators[1]];
										if (SQLGetData(hStmt, 15, SQL_C_DEFAULT, pPicture, pIndicators[1], &pIndicators[1]) != SQL_SUCCESS)
										{
											cout << "Failed to get a picture" << endl;
										}

										cout << "Column 15" << pPicture << endl;
										delete [] pPicture;
									}
									else
									{
										cout << "Error on attempt to get data length" << endl;
									}
								}
							}
						}

						SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
					}

					QueryPerformanceCounter(&end);
					QueryPerformanceFrequency(&freq);

					double result = (end.QuadPart - begin.QuadPart) / (double) freq.QuadPart;
					printf("%s : %4.2f ms\n", "ODBC - Time elapsed: ", result * 1000);

					SQLDisconnect(hDBC);
				}
				else
				{
					cout << "Failed to connect to the database" << endl;
				}
			
				SQLFreeHandle( SQL_HANDLE_DBC, hDBC );
			}
		}
	
		SQLFreeHandle( SQL_HANDLE_ENV, hEnv );

		cout<<"\nPress any key to exit";
		_getch();
		return 0;
	}
}