//#include "stdafx.h"
#include "Nomad.Data.h"

#include <concrt.h>
//#include <chrono>
//#include <thread>

//#include <iostream>
//#include <sstream>
//#include <string>
//#include <vector>
//#include <cstdio>
//#include <stdio.h>

//using std::cout;
//using std::endl;
//using std::string;
//using namespace System;
//using namespace System::Threading;


namespace Nomad
{
	namespace Data
	{
		//void Odbc::enroll(unsigned char *record, unsigned __int32 size) {
		//	//matcherFacadePtr->enroll(record, size);
		//	Nomad::Bio::MatcherFacade::enroll(record, size);
		//}

		bool terminateLoop;

		void Odbc::terminate(bool flag) {
			//matcherFacadePtr->enroll(record, size);
			terminateLoop = flag;
		}

		bool Odbc::getTerminationState() {
			//matcherFacadePtr->enroll(record, size);
			return terminateLoop;
		}

		BlockingCollection<int> *Odbc::_bc;


		//terminateLoop = false;
		//bool Odbc::terminateLoop = false;
		bool Odbc::fillOnly = false;

		Odbc::~Odbc() {
			//disconnect();
			if (hStmt != NULL) {
				SQLCloseCursor(hStmt);
				SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
			}

			if (hDBC != NULL) {
				SQLDisconnect(hDBC);
				SQLFreeHandle( SQL_HANDLE_DBC, hDBC );
			}
			
			if (hEnv != NULL)
				SQLFreeHandle( SQL_HANDLE_ENV, hEnv );

			if (matcherFacadePtr != NULL)
				delete matcherFacadePtr;

		}

		//Odbc::Odbc() {
		//	hEnv = SQL_NULL_HENV;
		//	hDBC = SQL_NULL_HDBC;
		//	strncpy_s((char *)ConnStrIn, sizeof(ConnStrIn) - 1, 
		//					"DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=MCCS_Egy;Trusted_Connection=yes;Mars_Connection=yes;",
		//					sizeof(ConnStrIn) - 1);		
		//}

		Odbc::Odbc(unsigned char *probeTemplate, unsigned __int32 probeTemplateSize, char* appSettings[], BlockingCollection<int>*bc) {
			hEnv = SQL_NULL_HENV;
			hDBC = SQL_NULL_HDBC;
			matcherFacadePtr = NULL;
			//buffer = NULL;
			//buffer2 = NULL;
			_bc = bc;

			if (probeTemplate != NULL) {
				matcherFacadePtr = new Nomad::Bio::MatcherFacade;
				matcherFacadePtr->enroll(probeTemplate, probeTemplateSize);
			}

			for(int i = 0; i < 4; i++) {
				dbSettings[i] = appSettings[i];
			}

			//if (appSettings[0] == ".")
			//	dbSettings[0] = "(local)";

			//"DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=MCCS_Egy;Trusted_Connection=yes;Mars_Connection=yes;"
			//std::stringstream stmt;
			//stmt << "DRIVER=ODBC Driver 11 for SQL Server;SERVER=" << appSettings[0] << ";DATABASE=" << appSettings[1] << ";Trusted_Connection=yes;Mars_Connection=yes;";

			strncpy_s((char*)ConnStrIn, sizeof(ConnStrIn) - 1, appSettings[0], sizeof(ConnStrIn) - 1);
			//strncpy_s((char*)ConnStrIn, sizeof(ConnStrIn) - 1, (char*)stmt.str().c_str(), sizeof(ConnStrIn) - 1);

			//strncpy_s((char *)ConnStrIn, sizeof(ConnStrIn) - 1, 
			//				"DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=MCCS_Egy;Trusted_Connection=yes;Mars_Connection=yes;",
			//				sizeof(ConnStrIn) - 1);

			//ConnStrIn = "DRIVER=ODBC Driver 11 for SQL Server;SERVER=(local);DATABASE=Northwind;Trusted_Connection=yes";
			//cbConnStrOut = 0;
		//}

		//SQLRETURN Odbc::connect(int *rowcount) {
			//SQLHSTMT hStmt = SQL_NULL_HSTMT;

			// Allocate environment handle
			rc = SQLAllocHandle( SQL_HANDLE_ENV, SQL_NULL_HANDLE, &hEnv );

			// Set the ODBC version environment attribute
			if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
/*				
if (!SQL_SUCCEEDED(SQLSetEnvAttr(
      NULL,  // make process level cursor pooling
      SQL_ATTR_CONNECTION_POOLING,
      (SQLPOINTER)SQL_CP_ONE_PER_DRIVER,
      SQL_IS_INTEGER)))
*/				
//				rc = SQLSetEnvAttr( hEnv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER*)SQL_OV_ODBC3, 0 );
				rc = SQLSetEnvAttr( hEnv, SQL_ATTR_ODBC_VERSION, (SQLPOINTER*)SQL_OV_ODBC3, SQL_IS_INTEGER );
				// Allocate connection handle
				if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
/*					
//set the matching condition for using an existing connection in the
   pool
   if (!SQL_SUCCEEDED(SQLSetEnvAttr(henv, SQL_ATTR_CP_MATCH,
   (SQLPOINTER) SQL_CP_RELAXED_MATCH, SQL_IS_INTEGER)))
   printf("SQLSetEnvAttr/SQL_ATTR_CP_MATCH error\n");
*/					
					rc = SQLAllocHandle( SQL_HANDLE_DBC, hEnv, &hDBC );
					// Set login timeout to 3 seconds
					if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
						SQLSetConnectAttr(hDBC, SQL_LOGIN_TIMEOUT, (SQLPOINTER)3, 0);
						SQLSetConnectAttr(hDBC, SQL_COPT_SS_MARS_ENABLED, (SQLPOINTER)SQL_MARS_ENABLED_YES, SQL_IS_UINTEGER);
						rc = SQLDriverConnect( hDBC, NULL, ConnStrIn, SQL_NTS, ConnStrOut, MAXBUFLEN, &cbConnStrOut, SQL_DRIVER_COMPLETE );
						if (!SQL_SUCCEEDED(rc) && rc != SQL_SUCCESS_WITH_INFO) {
							std::string errorMessage;
							extract_error("SQLDriverConnect", hDBC, SQL_HANDLE_DBC, &errorMessage);
							throw std::runtime_error(errorMessage.c_str());
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

		bool Odbc::getRowCount(unsigned __int32 *rowcount, std::string *errorMessage) {
			bool retcode = false;
			hStmt = SQL_NULL_HSTMT;
			rc = SQLAllocHandle( SQL_HANDLE_STMT, hDBC, &hStmt );
			if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {

			//std::stringstream stmt;
			//stmt << "SELECT count(*) FROM ";
			//for (int i = 0; i < numOfFieldsToMatch; i++) {
			//	if (i != 0)
			//		stmt << ",";

			//	stmt << fingerList[i];
			//}

			////stmt << " FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";
			//stmt << " FROM Egy_T_FingerPrint WITH (NOLOCK) WHERE datalength(AppWsq) IS NOT NULL ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";

				//"SELECT count(*) FROM Egy_T_FingerPrint WHERE datalength(AppWsq) IS NOT NULL"
				std::stringstream stmt;
				stmt << "SELECT count(*) FROM " << dbSettings[1] << " WHERE datalength(" << dbSettings[3] << ") IS NOT NULL";

				rc = SQLExecDirect(hStmt, (SQLCHAR*)stmt.str().c_str(), SQL_NTS);
				//rc = SQLExecDirect(hStmt, (SQLCHAR*)"SELECT count(*) FROM Egy_T_FingerPrint WHERE datalength(AppWsq) IS NOT NULL", SQL_NTS);
				//rc = SQLExecDirect(hStmt, (SQLCHAR*)"SELECT count(*) FROM Egy_T_AppPers", SQL_NTS);
				if (SQL_SUCCEEDED(rc) || rc == SQL_SUCCESS_WITH_INFO) {
					if ((rc = SQLFetch(hStmt)) == SQL_SUCCESS) {
						SQLGetData(hStmt, 1, SQL_C_SLONG, rowcount, sizeof(unsigned __int32), 0);
						SQLCloseCursor(hStmt);
						retcode = true;
					} else {
						extract_error("SQLExecDirect", hStmt, SQL_HANDLE_STMT, errorMessage);
					}
				} else {
					extract_error("SQLExecDirect", hStmt, SQL_HANDLE_STMT, errorMessage);
				}

				//SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
			}

			return retcode;
		}

		bool Odbc::getAppId(unsigned __int32 *appid, std::string *errorMessage) {
			bool retcode = false;
			hStmt = SQL_NULL_HSTMT;
			rc = SQLAllocHandle( SQL_HANDLE_STMT, hDBC, &hStmt );
			if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO) {
				std::stringstream stmt;
				stmt << "SELECT " << dbSettings[2] << " FROM " << dbSettings[1] << " WITH (NOLOCK) WHERE datalength(" << dbSettings[3] << ") IS NOT NULL ORDER BY " <<  dbSettings[2] << " ASC OFFSET " << *appid << " ROWS FETCH NEXT 1 ROWS ONLY ";
				//stmt << "SELECT AppID FROM Egy_T_FingerPrint WITH (NOLOCK) WHERE datalength(AppWsq) IS NOT NULL ORDER BY AppID ASC OFFSET " << *appid << " ROWS FETCH NEXT 1 ROWS ONLY ";
				rc = SQLExecDirect(hStmt, (SQLCHAR*)stmt.str().c_str(), SQL_NTS);
				if (SQL_SUCCEEDED(rc) || rc == SQL_SUCCESS_WITH_INFO) {
					if ((rc = SQLFetch(hStmt)) == SQL_SUCCESS) {
						SQLGetData(hStmt, 1, SQL_C_SLONG, appid, sizeof(unsigned __int32), 0);
						SQLCloseCursor(hStmt);
						retcode = true;
					} else {
						extract_error("SQLExecDirect", hStmt, SQL_HANDLE_STMT, errorMessage);
					}
				} else {
					extract_error("SQLExecDirect", hStmt, SQL_HANDLE_STMT, errorMessage);
				}

				//SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
			}

			return retcode;
		}

		struct FIELDSTRUCT {
			SQLCHAR f[BUFFERLEN];
			SQLLEN fInd;
		};

		struct RECORDSTRUCT {
			FIELDSTRUCT F1;
			FIELDSTRUCT F2;
			FIELDSTRUCT F3;
			FIELDSTRUCT F4;
			FIELDSTRUCT F5;
			FIELDSTRUCT F6;
			FIELDSTRUCT F7;
			FIELDSTRUCT F8;
			FIELDSTRUCT F9;
			FIELDSTRUCT F10;
		};

		unsigned __int32 Odbc::exec(unsigned long int from, unsigned int limit, char *fingerList[], __int32 numOfFieldsToMatch, std::string *errorMessage, fnCallBack callBack) {
			hStmt = SQL_NULL_HSTMT;

			SQLULEN			NumRowsFetched;
			SQLUSMALLINT*	RowStatus;

			//struct FIELDSTRUCT {
			//   SQLCHAR f[10000];
			//   SQLLEN fInd;
			//};

			//struct RECORDSTRUCT {
			//   FIELDSTRUCT F1;
			//   FIELDSTRUCT F2;
			//   FIELDSTRUCT F3;
			//   FIELDSTRUCT F4;
			//   FIELDSTRUCT F5;
			//   FIELDSTRUCT F6;
			//   FIELDSTRUCT F7;
			//   FIELDSTRUCT F8;
			//   FIELDSTRUCT F9;
			//   FIELDSTRUCT F10;
			//};
			RECORDSTRUCT* Record;

			//typedef struct {
			//	SQLLEN	imageIndicator;
			//	BYTE		pByte;
			//	PBYTE		pImage;
			//} IMAGESTRUCT;
			//IMAGESTRUCT ImageStruct;

//			matcherFacadePtr = new Nomad::Bio::MatcherFacade;

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
			rc = SQLSetStmtAttr(hStmt, SQL_SOPT_SS_CURSOR_OPTIONS, (SQLPOINTER)SQL_CO_FFO, SQL_IS_INTEGER);
			//rc = SQLSetStmtAttr(hStmt, SQL_ATTR_CURSOR_TYPE, (SQLPOINTER)SQL_CURSOR_FORWARD_ONLY, SQL_IS_INTEGER);
			//rc = SQLSetStmtAttr(hStmt, SQL_ATTR_CONCURRENCY, (SQLPOINTER)SQL_CONCUR_READ_ONLY, SQL_IS_INTEGER);
			rc = SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_ARRAY_SIZE, (SQLPOINTER)(limit + 1), SQL_IS_INTEGER);

			try {
				Record = new RECORDSTRUCT[limit + 1];
			} catch (std::bad_alloc& e) {
				*errorMessage = e.what();
				//FreeStmtHandle(hStmt);
				//delete matcherFacadePtr;
				return 0;							
			}

			try {
				RowStatus = new SQLUSMALLINT[limit + 1];
			}
			catch (std::bad_alloc& e) {
				*errorMessage = e.what();
				//FreeStmtHandle(hStmt);
				//delete matcherFacadePtr;
				return 0;
			}

			rc = SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_BIND_TYPE, (SQLPOINTER)sizeof(RECORDSTRUCT), SQL_IS_UINTEGER);
			rc = SQLSetStmtAttr(hStmt, SQL_ATTR_ROW_STATUS_PTR, RowStatus, SQL_IS_POINTER);
			rc = SQLSetStmtAttr(hStmt, SQL_ATTR_ROWS_FETCHED_PTR, &NumRowsFetched, 0);

			SQLBindCol(hStmt, 1, SQL_C_BINARY, &Record[0].F1.f, limit, &Record[0].F1.fInd);
			SQLBindCol(hStmt, 2, SQL_C_BINARY, &Record[0].F2.f, limit, &Record[0].F2.fInd);
			SQLBindCol(hStmt, 3, SQL_C_BINARY, &Record[0].F3.f, limit, &Record[0].F3.fInd);
			SQLBindCol(hStmt, 4, SQL_C_BINARY, &Record[0].F4.f, limit, &Record[0].F4.fInd);
			SQLBindCol(hStmt, 5, SQL_C_BINARY, &Record[0].F5.f, limit, &Record[0].F5.fInd);
			SQLBindCol(hStmt, 6, SQL_C_BINARY, &Record[0].F6.f, limit, &Record[0].F6.fInd);
			SQLBindCol(hStmt, 7, SQL_C_BINARY, &Record[0].F7.f, limit, &Record[0].F7.fInd);
			SQLBindCol(hStmt, 8, SQL_C_BINARY, &Record[0].F8.f, limit, &Record[0].F8.fInd);
			SQLBindCol(hStmt, 9, SQL_C_BINARY, &Record[0].F9.f, limit, &Record[0].F9.fInd);
			SQLBindCol(hStmt, 10, SQL_C_BINARY, &Record[0].F10.f, limit, &Record[0].F10.fInd);

			std::stringstream stmt;
			//stmt << "SELECT AppID, AppImage FROM Egy_T_AppPers WITH (NOLOCK) WHERE datalength(AppImage) IS NOT NULL ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";

			//stmt << "SELECT AppID, AppImage FROM Egy_T_AppPers WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";
			//stmt << "SELECT ri, rm FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";

			//if (numOfFieldsToMatch == 0) {
			//	numOfFieldsToMatch = 10;
			//stmt << "SELECT li, lm, lr, ll, ri, rm, rr, rl, lt, rt FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";
			//stmt << "SELECT ri, rm, rr FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";
			//}
			//else {
			stmt << "SELECT ";
			for (int i = 0; i < numOfFieldsToMatch; i++) {
				if (i != 0)
					stmt << ",";

				stmt << fingerList[i];
			}

			stmt << " FROM " << dbSettings[1] << " WITH (NOLOCK) WHERE datalength(" << dbSettings[3] << ") IS NOT NULL ORDER BY " << dbSettings[2] << " ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";

			//stmt << " FROM Egy_T_FingerPrint WITH (NOLOCK) ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";
			//stmt << " FROM Egy_T_FingerPrint WITH (NOLOCK) WHERE datalength(AppWsq) IS NOT NULL ORDER BY AppID ASC OFFSET " << from << " ROWS FETCH NEXT " << limit << " ROWS ONLY ";
			//}

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

			rc = SQLExecDirect(hStmt, (SQLCHAR*)stmt.str().c_str(), SQL_NTS);
			if (!(rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO))
			{
				extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT, errorMessage);
				delete[] RowStatus;
				delete[] Record;
				//FreeStmtHandle(hStmt);
				//delete matcherFacadePtr;
				return 0;
			}

			bool matched = false;
			unsigned __int32 RowNumber = 0;
			short numOfFieldsInRecord = sizeof(Record[0]) / sizeof(FIELDSTRUCT);
			while ((rc = SQLFetchScroll(hStmt, SQL_FETCH_NEXT, 0)) != SQL_NO_DATA) {
				if (!(rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO))
				{
					extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT, errorMessage);
					delete[] RowStatus;
					delete[] Record;
					//FreeStmtHandle(hStmt);
					//delete matcherFacadePtr;
					throw std::runtime_error((*errorMessage).c_str());
					return 0;
				}
				//if (rc == SQL_SUCCESS_WITH_INFO) {
				//	std::stringstream ss; 
				//	ss << "Oversized template:";
				//	Log(ss.str(), false);
				//}

				if (getTerminationState())
					break;

				short numOfMatches = 0;
				for (SQLUSMALLINT i = 0; i < NumRowsFetched; i++) {

					if (i % 1000 == 0 && _bc != NULL) {
						_bc->push(1000);
					}

					numOfMatches = 0;
					matched = false;

					if (getTerminationState())
						break;

					if (fillOnly)
						continue;

					FIELDSTRUCT* f;

					for (SQLUSMALLINT j = 0; j < numOfFieldsToMatch; j++) {
						if (RowStatus[i] == SQL_ROW_SUCCESS) {
							//matched = true;
							//numOfMatches++;
							f = &Record[0].F1 + j + (i * numOfFieldsInRecord);

							if (f->fInd != SQL_NULL_DATA && f->fInd != 0) {
								try {

									//if (i == 708 || i == 5012) {
									//if (i == 708 || i == 58) {
									//	//continue;
									//	int kk = 0;

									//}

									//matched = matcherFacadePtr->match(RecordStructArray[i].li, static_cast<unsigned __int32>(RecordStructArray[i].liInd));
									//matched = matcherFacadePtr->match(f->f, static_cast<unsigned __int32>(*index));

									//if (!fillOnly)
										matched = matcherFacadePtr->match(f->f, static_cast<unsigned __int32>(f->fInd));

									if (matched) {
										numOfMatches++;
									}

									//if (1) {
									//	std::stringstream ss; 
									//	ss << "r: " << j << " : " << i << " from: " << from;
									//	Log(ss.str(), false);
									//}

								} catch (std::exception& e) {
									*errorMessage = e.what();
									delete[] RowStatus;
									delete[] Record;
									//FreeStmtHandle(hStmt);
									//delete matcherFacadePtr;
									return 0;							
								}
							}
						} else if (RowStatus[i] == SQL_ROW_SUCCESS_WITH_INFO) {
							//if (i == 146 || i == 171) {
							//	int kk = 0;
							//}

							//f = &Record[0].F1 + j + (i * numOfFieldsInRecord);
							//std::stringstream ss; 
							//ss << "Oversized template:" << (f->fInd) << " : " << j << " : " << i << " from: " << from;
							//Log(ss.str(), false);

/*							*errorMessage = "An error retrieving the row from the data source with SQLFetch";
							delete[] RowStatus;
							delete[] Record;
							FreeStmtHandle(hStmt);
							delete matcherFacadePtr;
							return 0;
*/	
						} else if (RowStatus[i] == SQL_ROW_ERROR) {
							*errorMessage = "An error retrieving the row from the data source with SQLFetch";
							delete[] RowStatus;
							delete[] Record;
							//FreeStmtHandle(hStmt);
							//delete matcherFacadePtr;
							return 0;				
						}
					}

					if (numOfFieldsToMatch == numOfMatches) {
						RowNumber = i + from + 1;
						break;
					}

/*
					// Call SQLGetData to determine the amount of data that's waiting.
					rc = SQLSetPos(hStmt, i + 1, SQL_POSITION, SQL_LOCK_NO_CHANGE);
					if (rc == SQL_SUCCESS || rc == SQL_SUCCESS_WITH_INFO)
					{
						//short numOfFieldsToMatch = 2;
						short numOfMatches = 0;
						for (int j = 1; j < numOfFieldsToMatch + 1; j++) {
							//rc = SQLGetData(hStmt, j, SQL_C_BINARY, (SQLPOINTER)&ImageStruct.pByte, 0, &ImageStruct.imageIndicator);
							if ((rc = SQLGetData(hStmt, j, SQL_C_BINARY, (SQLPOINTER)&ImageStruct.pByte, 0, &ImageStruct.imageIndicator)) == SQL_SUCCESS_WITH_INFO)
							{
								//if (ImageStruct.imageIndicator > 10000) {
								//	std::stringstream ss; 
								//	ss << "Image size: " << i + 1 << " : " << ImageStruct.imageIndicator << std::endl;
								//	OutputDebugString(ss.str().c_str());
								//}

								// Get all the data at once.
								ImageStruct.pImage = new BYTE[ImageStruct.imageIndicator];
								if (SQLGetData(hStmt, j, SQL_C_BINARY, ImageStruct.pImage, ImageStruct.imageIndicator, &ImageStruct.imageIndicator) == SQL_SUCCESS)
								{
									matched = matcherFacadePtr->match(ImageStruct.pImage, static_cast<unsigned __int32>(ImageStruct.imageIndicator));
								} else {
									extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT, errorMessage);
									FreeStmtHandle(hStmt);
									delete matcherFacadePtr;
									return 0;
								}

								delete [] ImageStruct.pImage;

								if (matched) {
									numOfMatches++;
									if (numOfFieldsToMatch == numOfMatches) {
										RowNumber = i + from + 1;
										break;
									} else
										matched = false;
								}
							} else if (ImageStruct.imageIndicator != 0 && ImageStruct.imageIndicator != SQL_NULL_DATA) {
								extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT, errorMessage);
								//delete[] RowStatusArray;
								//delete[] AppIDStructArray;
								FreeStmtHandle(hStmt);
								delete matcherFacadePtr;
								return 0;
							}
						}

						if (matched)
							break;

					} else {
						extract_error("SQLExecute", hStmt, SQL_HANDLE_STMT, errorMessage);
						//delete[] RowStatusArray;
						//delete[] AppIDStructArray;
						FreeStmtHandle(hStmt);
						delete matcherFacadePtr;
						return 0;
					}

*/
				}

				if (matched)
					break;

				if (getTerminationState())
					break;
			}

			delete[] RowStatus;
			delete[] Record;
			//FreeStmtHandle(hStmt);
			//delete matcherFacadePtr;

			if (_bc != NULL) {
				if (getTerminationState())
					_bc->push(-2);
				else
					_bc->push(-1);
			}

			return RowNumber;
		}

		//void Odbc::FreeStmtHandle(SQLHSTMT hStmt) {
		//	if (hStmt != NULL) {
		//		SQLCloseCursor(hStmt);
		//		SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
		//	}
		//}

		//void Odbc::disconnect() {
		//	//if (hStmt != NULL) {
		//	//	SQLCloseCursor(hStmt);
		//	//	SQLFreeHandle(SQL_HANDLE_STMT, hStmt);
		//	//}

		//	if (hDBC != NULL) {
		//		SQLDisconnect(hDBC);
		//		SQLFreeHandle( SQL_HANDLE_DBC, hDBC );
		//	}
		//	
		//	if (hEnv != NULL)
		//		SQLFreeHandle( SQL_HANDLE_ENV, hEnv );
		//}

		//void Odbc::extract_error(char *fn, SQLHANDLE handle, SQLSMALLINT type)
		//{
		//	extract_error(fn, handle, type, NULL);
		//}

		void Odbc::extract_error(char *fn, SQLHANDLE handle, SQLSMALLINT type, std::string *errorMessage)
		{
			SQLSMALLINT	i = 0;
			SQLINTEGER	native;
			SQLCHAR		state[ 7 ];
			SQLCHAR		text[256];
			SQLSMALLINT	len;
			SQLRETURN	ret;
			std::stringstream ss; 


			//std::streambuf *psbuf, *backup;
			//std::ofstream filestr;
			//filestr.open("error.txt");
			//backup = std::cout.rdbuf();     // back up cout's streambuf
			//psbuf = filestr.rdbuf();        // get file's streambuf
			//std::cout.rdbuf(psbuf);         // assign streambuf to cout
			//std::cout << "The driver reported the following diagnostics whilst running " << fn << std::endl;
			//std::cout.rdbuf(backup);        // restore cout's original streambuf
			//filestr.close();



			//fprintf(stdout,
			//		"\n"
			//		"The driver reported the following diagnostics whilst running "
			//		"%s\n\n",
			//		fn);

			ss << "The driver reported the following diagnostics whilst running " << fn << std::endl;
//			printStatusStatement("The driver reported the following diagnostics whilst running " + fn);
			do
			{
				ret = SQLGetDiagRec(type, handle, ++i, state, &native, text, sizeof(text), &len );
				if (SQL_SUCCEEDED(ret))
					ss << state << ":" << i << ":"  << native << ":" << text << std::endl;
					//printf("%s:%ld:%ld:%s\n", state, i, native, text);
			}
			while( ret == SQL_SUCCESS );

			//int ii = ss.str().length();

			*errorMessage = ss.str();
		}
	}
}