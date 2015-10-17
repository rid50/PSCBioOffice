//#include "stdafx.h"
#ifndef NOMAD_DATA_ODBC_H
#define NOMAD_DATA_ODBC_H
//#include <NCore.h>
//#include <NLicensing.h>

//#include "stdafx.h"
//#include <WinDef.h>
//#include <vector>
//#include <malloc.h>

#include <Windows.h>
//#include <memory>

#include <string>

#include <sql.h>
#include <sqlext.h>
#include <sqltypes.h>
#include <sqlncli.h>
#include <iostream>     // std::streambuf, std::cout
#include <fstream>      // std::ofstream
#include <sstream>		// std::stringstream

//#include <conio.h>

//#include <NFExtractor.h>
//#include <NMatcher.h>

#pragma comment(lib, "odbc32.lib")
#pragma comment(lib, "user32.lib")
#pragma comment(lib, "Matcher.lib")

//using namespace std;

//void getLicenses();
//inline void printStatusStatement(char * statusStatement);

namespace Nomad
{
	//__declspec(dllimport) class Matcher {};
	//__declspec(dllimport) void __stdcall  enroll();
	//__declspec(dllimport) void __stdcall match(void *buffer2, int size);

	namespace Bio
	{
		class __declspec(dllimport) MatcherFacade
		{
		public:
			void* matcherPtr;
		public:
			MatcherFacade();
			~MatcherFacade();
			static void enroll(unsigned char *record, unsigned __int32 size);
			static void terminateLoop(bool terminateLoop);
			bool match(void *prescannedTemplate, int prescannedTemplateSize);
		};
	}

	namespace Data
	{
		class Odbc
		{
#define MAXBUFLEN   255
			//#define ROW_ARRAY_SIZE 100000

		private:
			//Nomad::Bio::MatcherFacade matcherFacade;
			Nomad::Bio::MatcherFacade	*matcherFacadePtr;
			static bool	shallTerminateLoop;

			//void *buffer;
			//void *buffer2;
			//bool matchingStatus;
			//typedef pair <int, HNImage> imageMapPair;
			//std::map <int, HNImage> imageMap;

		private:
			//void printStatusStatement(char * statusStatement);
			void FreeStmtHandle(SQLHSTMT hStmt);
			void extract_error(char *fn, SQLHANDLE handle, SQLSMALLINT type);
			void extract_error(char *fn, SQLHANDLE handle, SQLSMALLINT type, std::string *errorMessage);
			//void clean();
			//void TimedRun(Func test, string label);

			//public:

			SQLHENV hEnv;
			SQLHDBC hDBC;
			//SQLHSTMT hStmt;
			//char ConnStrIn[MAXBUFLEN];
			SQLCHAR ConnStrIn[MAXBUFLEN];
			SQLCHAR ConnStrOut[MAXBUFLEN];
			SQLSMALLINT cbConnStrOut;
			SQLRETURN rc;



		public:
			Odbc();
			~Odbc();
			//SQLRETURN connect(int*);
			bool getRowCount(unsigned __int32 *rowcount, std::string *errorMessage);
			//SQLRETURN exec();
			//SQLRETURN exec(unsigned int, unsigned int, unsigned int);
			static void enroll(unsigned char *record, unsigned __int32 size);
			static void terminateLoop(bool terminateLoop);
			unsigned __int32 exec(unsigned long int, unsigned int);
			void disconnect();

			//void readImages(wchar_t * szFileName, wchar_t * szFileName2);
			//void run();
			//bool getMatchingStatus();


		private:
			/// <summary>
			/// Executes a function and prints timing results
			/// </summary>
			template<typename Func>
			void TimedRun(Func test, std::string label)
			{
				LARGE_INTEGER begin, end, freq;
				QueryPerformanceCounter(&begin);

				// invoke the function
				test();

				// print timings
				QueryPerformanceCounter(&end);

				QueryPerformanceFrequency(&freq);

				double result = (end.QuadPart - begin.QuadPart) / (double) freq.QuadPart;
				printf("%s : %4.2f ms\n", label.c_str(), result * 1000);

				//return result;
			}

			inline void printStatusStatement(char * statusStatement) {
#ifdef _DEBUG
				//sprintf_s (ServiceListener::statusStatement, statusStatement);
				std::cout << statusStatement << std::endl;
#endif
			}

			inline void printStatusStatement(int statusStatement) {
#ifdef _DEBUG
				//sprintf_s (ServiceListener::statusStatement, statusStatement);
				std::cout << statusStatement << std::endl;
#endif
			}

		};
	}
}
#endif
