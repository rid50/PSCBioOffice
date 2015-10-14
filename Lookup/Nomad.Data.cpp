//#include "stdafx.h"
#include "Nomad.Data.h"

//#include <iostream>
//#include <sstream>
//#include <string>
//#include <vector>
//#include <cstdio>
//#include <stdio.h>

using std::cout;
using std::endl;

namespace Nomad
{
	namespace Data
	{
		void Odbc::enroll(unsigned char *record, unsigned __int32 size) {
			//matcherFacadePtr->enroll(record, size);
			Nomad::Bio::MatcherFacade::enroll(record, size);
		}

		Odbc::~Odbc() {
			disconnect();
		}

		Odbc::Odbc() {
			//buffer = NULL;
			//buffer2 = NULL;

			hEnv = SQL_NULL_HENV;
			hDBC = SQL_NULL_HDBC;
			strncpy_s((char *)ConnStrIn, sizeof(ConnStrIn) - 1, 
							"DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=MCCS_Egy;Trusted_Connection=yes;Mars_Connection=yes;",
							sizeof(ConnStrIn) - 1);

			//ConnStrIn = "DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=Northwind;Trusted_Connection=yes";
			//cbConnStrOut = 0;
		//}

		//SQLRETURN Odbc::connect(int *rowcount) {
			//SQLHSTMT hStmt = SQL_NULL_HSTMT;

			// Allocate environment handle
			rc = SQLAllocHandle( SQL_HANDLE_ENV, SQL_NULL_HANDLE, &hEnv );

			// Set the ODBC version environment attribute
			if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
				rc = SQLSetEnvAttr( hEnv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER*)SQL_OV_ODBC3, 0 );

				// Allocate connection handle
				if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
					rc = SQLAllocHandle( SQL_HANDLE_DBC, hEnv, &hDBC );

					// Set login timeout to 3 seconds
					if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
						SQLSetConnectAttr(hDBC, SQL_LOGIN_TIMEOUT, (SQLPOINTER)3, 0);
						SQLSetConnectAttr(hDBC, SQL_COPT_SS_MARS_ENABLED, (SQLPOINTER)SQL_MARS_ENABLED_YES, SQL_IS_UINTEGER);
						rc = SQLDriverConnect( hDBC, NULL, ConnStrIn, SQL_NTS, ConnStrOut, MAXBUFLEN, &cbConnStrOut, SQL_DRIVER_COMPLETE );
						if (!SQL_SUCCEEDED(rc) && rc != SQL_SUCCESS_WITH_INFO)						{
							extract_error("SQLDriverConnect", hDBC, SQL_HANDLE_DBC);
							//return 1;
						}
						//else {
						//	rc = SQLAllocHandle( SQL_HANDLE_STMT, hDBC, &hStmt );
						//	if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
						//		extract_error("SQLAllocHandle", hDBC, SQL_HANDLE_DBC);
						//		//return -1;

						//		//SQLExecDirect(hStmt, (SQLCHAR*)"SELECT count(*) FROM Egy_T_AppPers", SQL_NTS);
						//		//if (SQLFetch(hStmt) == SQL_SUCCESS) {
						//		//	SQLGetData(hStmt, 1, SQL_C_SLONG, rowcount, sizeof(int), 0);
						//		//	SQLCloseCursor(hStmt);
						//		//	SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
						//		//}
						//	}
						//}
					}
				}		
			}
			
			//return 0;
		}

		bool Odbc::getRowCount(int *rowcount) {
			bool retcode = false;
			SQLHSTMT hStmt = SQL_NULL_HSTMT;
			rc = SQLAllocHandle( SQL_HANDLE_STMT, hDBC, &hStmt );
			if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
				SQLExecDirect(hStmt, (SQLCHAR*)"SELECT count(*) FROM Egy_T_AppPers", SQL_NTS);
				if (SQLFetch(hStmt) == SQL_SUCCESS) {
					SQLGetData(hStmt, 1, SQL_C_SLONG, rowcount, sizeof(int), 0);
					SQLCloseCursor(hStmt);
					retcode = true;
				}

				SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
			}

			return retcode;
		}


		//SQLRETURN Odbc::exec(unsigned int from, unsigned int to, unsigned int limit) {
		unsigned __int32 Odbc::exec(unsigned long int from, unsigned int limit) {

			SQLHSTMT hStmt = SQL_NULL_HSTMT;

			SQLUINTEGER		NumRowsFetched;
			//SQLUINTEGER		AppIDArray[ROW_ARRAY_SIZE];
			//SQLINTEGER		AppIDIndArray[ROW_ARRAY_SIZE];

			//SQLUSMALLINT	RowStatusArray[ROW_ARRAY_SIZE];
			SQLUSMALLINT*	RowStatusArray;

			struct APPIDSTRUCT {
			   SQLUINTEGER   AppID;
			   SQLINTEGER    AppIDInd;
			};
			APPIDSTRUCT* AppIDStructArray;
			//APPIDSTRUCT AppIDStructArray[ROW_ARRAY_SIZE];

			typedef struct {
				SQLINTEGER	imageIndicator;
				BYTE		pByte;
				PBYTE		pImage;
			} IMAGESTRUCT;
			IMAGESTRUCT ImageStruct;

			matcherFacadePtr = new Nomad::Bio::MatcherFacade;

/*
SQLSetStmtAttr(hstmt, SQL_ATTR_CURSOR_TYPE, SQL_CURSOR_FORWARD_ONLY, SQL_IS_INTEGER);
SQLSetStmtAttr(hstmt, SQL_ATTR_CONCURRENCY, SQL_CONCUR_READ_ONLY, SQL_IS_INTEGER);
SQLSetStmtAttr(hstmt, SQL_ATTR_ROW_ARRAY_SIZE, 1, SQL_IS_INTEGER);
*/
			//std::string from = "0", to = "100000";
			/*
			const char stmt_to_format[] = "SELECT FirstName FROM (SELECT ROW_NUMBER() OVER(ORDER BY FirstName) AS row, FirstName FROM Employees) r WHERE row > %s and row <= %s";
			// use std::vector for your memory management (to avoid memory leaks)
			std::vector<char> stmt;
			// make sure to have a large enough buffer
			stmt.resize(from.length() + to.length() + sizeof(stmt_to_format));
			// use snprintf instead of sprintf (to avoid buffer overflows)
			std::snprintf(&stmt[0], stmt.size(), stmt_to_format, from.c_str(), to.c_str());
			// assign to std::string
			std::string str = &stmt[0];
			//std::cout << str << "\n";
			*/
			//char * stmt = "SELECT FirstName FROM Employees";
            //char * stmt = "SELECT FirstName FROM (SELECT ROW_NUMBER() OVER(ORDER BY FirstName) AS row, FirstName FROM Employees) r WHERE row > 2 and row <= 5";
            //cmd.CommandText = String.Format("SELECT AppID, AppWsq FROM (SELECT ROW_NUMBER() OVER(ORDER BY AppID) AS row, AppID, AppWsq FROM Egy_T_FingerPrint WHERE datalength(AppWsq) IS NOT NULL) r WHERE row > {0} and row <= {1}", from, to);
            //cmd.CommandText = String.Format("SELECT AppID, AppWsq FROM Egy_T_FingerPrint WITH (NOLOCK) WHERE datalength(AppWsq) IS NOT NULL ORDER BY AppID ASC OFFSET {0} ROWS FETCH NEXT {1} ROWS ONLY ", from, count);
			//sprintf(x, "%s %s > %s", a.c_str(), b.c_str(), c.c_str() );

			rc = SQLAllocHandle(SQL_HANDLE_STMT, hDBC, &hStmt);
			if (!SQL_SUCCEEDED(rc) || rc == SQL_SUCCESS_WITH_INFO)
			{
				extract_error("SQLAllocHandle", hDBC, SQL_HANDLE_DBC);
				delete matcherFacadePtr;
				return -1;
			}

			SQLSetStmtAttr(hStmt, SQL_ATTR_CURSOR_TYPE, (SQLPOINTER)SQL_CURSOR_FORWARD_ONLY, SQL_IS_INTEGER);
			SQLSetStmtAttr(hStmt, SQL_ATTR_CONCURRENCY, (SQLPOINTER)SQL_CONCUR_READ_ONLY, SQL_IS_INTEGER);
			SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_ARRAY_SIZE, (SQLPOINTER)limit, SQL_IS_INTEGER);
			//SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_ARRAY_SIZE, (SQLPOINTER)ROW_ARRAY_SIZE, SQL_IS_INTEGER);

			RowStatusArray = new SQLUSMALLINT[limit];
			AppIDStructArray = new APPIDSTRUCT[limit];

			SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_BIND_TYPE, (SQLPOINTER)sizeof(APPIDSTRUCT), 0);
			//SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_BIND_TYPE, SQL_BIND_BY_COLUMN, 0);
			SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_STATUS_PTR, RowStatusArray, 0);
			SQLSetStmtAttr(hStmt, SQL_ATTR_ROWS_FETCHED_PTR, &NumRowsFetched, 0);

			// Bind arrays to the AppID.
			SQLBindCol(hStmt, 1, SQL_C_ULONG, &AppIDStructArray[0].AppID, 0, &AppIDStructArray[0].AppIDInd);
			//SQLBindCol(hStmt, 1, SQL_C_ULONG, AppIDArray, 0, AppIDIndArray);

			std::stringstream stmt;
			//stmt << "SELECT AppID, AppImage FROM Egy_T_AppPers WITH (NOLOCK) WHERE datalength(AppImage) IS NOT NULL ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";

			//stmt << "SELECT AppID, AppImage FROM Egy_T_AppPers WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";
			stmt << "SELECT AppID, li FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";

			//stmt << "SELECT AppID, AppImage FROM (SELECT ROW_NUMBER() OVER(ORDER BY AppID) AS row, AppID, AppImage FROM Egy_T_AppPers WHERE datalength(AppImage) IS NOT NULL) r WHERE row > " << from << " and row <= " << to << "\n";
			//stmt << "SELECT AppID FROM (SELECT ROW_NUMBER() OVER(ORDER BY AppID) AS row, AppID FROM Egy_T_AppPers) r WHERE row > " << from << "\n";
			//stmt << "SELECT TOP 100000 AppID, AppImage FROM Egy_T_AppPers ORDER BY AppID ASC";
			//stmt << "SELECT AppID, AppImage FROM Egy_T_AppPers ORDER BY AppID ASC";
			//cout << (SQLCHAR*)stmt.str().c_str() << endl;

/*
			//SQLPrepare(hStmt, (SQLCHAR *)"SELECT AppID, AppImage FROM (SELECT ROW_NUMBER() OVER(ORDER BY AppID) AS row, AppID, AppImage FROM Egy_T_AppPers WHERE datalength(AppImage) IS NOT NULL) r WHERE row > ? and row <= ?", SQL_NTS);
			//SQLPrepare(hStmt, (SQLCHAR *)"SELECT AppID, AppImage FROM Egy_T_AppPers WITH (NOLOCK) WHERE datalength(AppImage) IS NOT NULL ORDER BY AppID ASC OFFSET ? ROWS FETCH NEXT ? ROWS ONLY", SQL_NTS);
			
			SQLPrepare(hStmt, (SQLCHAR *)"SELECT AppImage FROM Egy_T_AppPers WITH (NOLOCK) ORDER BY AppID ASC OFFSET ? ROWS FETCH NEXT ? ROWS ONLY", SQL_NTS);
			
			//SQLPrepare(hStmt, (SQLCHAR *)"SELECT AppID, AppImage FROM Egy_T_AppPers WHERE AppID > ? ORDER BY AppID ASC", SQL_NTS);

			SQLUINTEGER	Param = from, Param2 = limit;
			SQLINTEGER	ParamInd = 0, Param2Ind = 0;
			SQLBindParameter(hStmt, 1, SQL_PARAM_INPUT, SQL_C_ULONG, SQL_INTEGER, 10, 0, &Param, 0, &ParamInd);
			SQLBindParameter(hStmt, 2, SQL_PARAM_INPUT, SQL_C_ULONG, SQL_INTEGER, 10, 0, &Param2, 0, &Param2Ind);
*/
			// -- CHECKPOINT
			// DBCC DROPCLEANBUFFERS() 

			//cout << "Execute started... "<< endl;

			//rc = SQLExecute(hStmt);

			rc = SQLExecDirect(hStmt, (SQLCHAR*)stmt.str().c_str(), SQL_NTS);
			if (!SQL_SUCCEEDED(rc) && rc != SQL_SUCCESS_WITH_INFO)
			{
				extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT);
				delete[] RowStatusArray;
				delete[] AppIDStructArray;
				FreeStmtHandle(hStmt);
				delete matcherFacadePtr;
				return -1;
			}

			//cout << "from: " << from << endl;
			//cout << "limit: " << limit << endl;
			SQLUINTEGER n = 0;
			bool matched = false;
			unsigned __int32 AppId = 0;
			while ((rc = SQLFetchScroll(hStmt, SQL_FETCH_NEXT, 0)) != SQL_NO_DATA) {
				//cout << "Number of rows: " << n << endl;
				n = NumRowsFetched - 1;
				for (SQLUSMALLINT i = 0; i < NumRowsFetched; i++) {
					if (RowStatusArray[i] == SQL_ROW_SUCCESS) {

						//if (AppIDStructArray[i].AppIDInd == SQL_NULL_DATA)
						//	printf(" NULL      ");
						//else
						//printf("%d\t", AppIDStructArray[i].AppID);
						
						//std::cout << AppIDStructArray[i].AppID << std::endl;

						//if (AppIDIndArray[i] == SQL_NULL_DATA)
						//	printf(" NULL      ");
						//else
						//	printf("%d\t", AppIDArray[i]);
					}

					// Call SQLGetData to determine the amount of data that's waiting.
					//pPicture = new BYTE[1];
					
					rc = SQLSetPos(hStmt, i + 1, SQL_POSITION, SQL_LOCK_NO_CHANGE);
					if (!(rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO))
					{
						extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT);
					}

					rc = SQLGetData(hStmt, 2, SQL_C_BINARY, (SQLPOINTER)&ImageStruct.pByte, 0, &ImageStruct.imageIndicator);
					if ((rc = SQLGetData(hStmt, 2, SQL_C_BINARY, (SQLPOINTER)&ImageStruct.pByte, 0, &ImageStruct.imageIndicator)) == SQL_SUCCESS_WITH_INFO)
					//if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO)
					{
						//std::cout << "Photo size: " << imageIndicator << "\n\n";

						// Get all the data at once.
						ImageStruct.pImage = new BYTE[ImageStruct.imageIndicator];
						if (SQLGetData(hStmt, 2, SQL_C_BINARY, ImageStruct.pImage, ImageStruct.imageIndicator, &ImageStruct.imageIndicator) == SQL_SUCCESS)
						{
							matched = matcherFacadePtr->match(ImageStruct.pImage, ImageStruct.imageIndicator);
							//if (ismatched)
								//std::cout << "AppId: " << AppIDStructArray[i].AppID << " --- templates matched " << std::endl;

							//std::cout << "AppId: " << AppIDStructArray[i].AppID << " - data length: " << ImageStruct.imageIndicator << std::endl;

							//matcherFacade.match(static_cast<void*>(ImageStruct.pImage), static_cast<int>(ImageStruct.imageIndicator));
						} else {
							extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT);
						}

						//std::cout << "Column 2" << pImage << std::endl;
						delete [] ImageStruct.pImage;
						if (matched) {
							AppId = AppIDStructArray[i].AppID;
							break;
						}
					//} else if (rc == SQL_NO_DATA) {
					//	//extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT);
					//	std::cout << "AppId: " << AppIDStructArray[i].AppID << " - No data(SQLGetData)" << std::endl;
					//} else if (ImageStruct.imageIndicator == SQL_NO_DATA) {
					//	//extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT);
					//	std::cout << "AppId: " << AppIDStructArray[i].AppID << " - No data (indicator)" << std::endl;
					//} else if (ImageStruct.imageIndicator == SQL_NO_TOTAL) {
					//	//extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT);
					//	std::cout << "AppId: " << AppIDStructArray[i].AppID << " - No total" << std::endl;
					//} else if (ImageStruct.imageIndicator == SQL_NULL_DATA ) {
					//	std::cout << "AppId: " << AppIDStructArray[i].AppID << " - NULL data" << std::endl;
					//} else {
					//	std::cout << "AppId: " << AppIDStructArray[i].AppID << " - data length: " << ImageStruct.imageIndicator << std::endl;
					}
					

				}
//				cout << "----------------------" << endl;

				if (matched)
					break;
			}

			std::cout << AppIDStructArray[n].AppID << endl;
			delete[] RowStatusArray;
			delete[] AppIDStructArray;
			FreeStmtHandle(hStmt);
			delete matcherFacadePtr;

/*
			if (rc == SQL_SUCCESS) {
				SQLSMALLINT nCols = 0;
				//SQLINTEGER	nRows = 0;
				//SQLINTEGER	nIdicator = 0;
				SQLCHAR		buf[1024];
				BYTE		pByte;
				PBYTE		pPicture;
				SQLINTEGER  pIndicators[2];

				SQLNumResultCols(hStmt, &nCols);
				//SQLRowCount(hStmt, &nRows);
				int n = 0;
				//while(SQLFetch( hStmt ) == SQL_SUCCESS)
				while(TRUE)
				{
					rc = SQLFetch( hStmt );
					if (rc != SQL_SUCCESS) {
						//cout << rc << endl;
						cout << "Number of rows: " << n << endl;
						break;
					} else {
						for( int i = 1; i <= nCols; ++i )
						{
							//cout << "Number of rows fetched: " << NumRowsFetched << endl;
							n += NumRowsFetched;
							if (i != 15) {
								rc = SQLGetData(hStmt,	i, SQL_C_CHAR, buf, 1024, &pIndicators[0] );
								if(SQL_SUCCEEDED(rc))
								{
									//cout << ++n << ": " << buf << endl;
									//cout << "Column " << buf << endl;
								}
							}
							else
							{
								// Call SQLGetData to determine the amount of data that's waiting.
								//pPicture = new BYTE[1];
								if (SQLGetData(hStmt, 15, SQL_C_BINARY, (SQLPOINTER)&pByte, 0, &pIndicators[1]) == SQL_SUCCESS_WITH_INFO)
								{
									std::cout << "Photo size: " << pIndicators[1] << "\n\n";

									// Get all the data at once.
									pPicture = new BYTE[pIndicators[1]];
									if (SQLGetData(hStmt, 15, SQL_C_DEFAULT, pPicture, pIndicators[1], &pIndicators[1]) != SQL_SUCCESS)
									{
										std::cout << "Failed to get a picture" << std::endl;
									}

									std::cout << "Column 15" << pPicture << std::endl;
									delete [] pPicture;
								}
								else
								{
									std::cout << "Error on attempt to get data length" << std::endl;
								}
							}
						}
					}
				}
			}
*/
			return AppId;
		}

		void Odbc::FreeStmtHandle(SQLHSTMT hStmt) {
			if (hStmt != NULL) {
				SQLCloseCursor(hStmt);
				SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
			}
		}

		void Odbc::disconnect() {
			//if (hStmt != NULL) {
			//	SQLCloseCursor(hStmt);
			//	SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
			//}

			if (hDBC != NULL) {
				SQLDisconnect(hDBC);
				SQLFreeHandle( SQL_HANDLE_DBC, hDBC );
			}
			
			if (hEnv != NULL)
				SQLFreeHandle( SQL_HANDLE_ENV, hEnv );
		}

		void Odbc::extract_error(char *fn, SQLHANDLE handle, SQLSMALLINT type)
		{
			SQLSMALLINT	i = 0;
			SQLINTEGER	native;
			SQLCHAR		state[ 7 ];
			SQLCHAR		text[256];
			SQLSMALLINT	len;
			SQLRETURN	ret;

			fprintf(stderr,
					"\n"
					"The driver reported the following diagnostics whilst running "
					"%s\n\n",
					fn);

			do
			{
				ret = SQLGetDiagRec(type, handle, ++i, state, &native, text, sizeof(text), &len );
				if (SQL_SUCCEEDED(ret))
					printf("%s:%ld:%ld:%s\n", state, i, native, text);
			}
			while( ret == SQL_SUCCESS );
		}
	}
}