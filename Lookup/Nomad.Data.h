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

#define BUFFERLEN		20000
#define MESSAGE_SIZE	256

//using namespace std;

//void getLicenses();
//inline void printStatusStatement(char * statusStatement);

namespace Nomad
{
	//__declspec(dllimport) class Matcher {};
	//__declspec(dllimport) void __stdcall  enroll();
	//__declspec(dllimport) void __stdcall match(void *buffer2, int size);

	typedef struct _tagCallBackStruct
	{
		short code;
		wchar_t text[MESSAGE_SIZE]; 
	} CallBackStruct;

	typedef void (__stdcall *fnCallBack)(CallBackStruct* callBackParam);

	namespace Bio
	{
		class __declspec(dllimport) MatcherFacade
		{
		public:
			void* matcherPtr;
		public:
			MatcherFacade();
			~MatcherFacade();
			void enroll(unsigned char *probeTemplate, unsigned __int32 probeTemplateSize);
			//static void terminateLoop();
			bool match(void *galleryTemplate, unsigned __int32 galleryTemplateSize);
		};
	}

	namespace Data
	{
		inline void Log(std::string str, bool file)
		{
			if (file) {
				std::streambuf *psbuf, *backup;
				std::ofstream filestr;
				filestr.open("c:\\temp\\error.txt", std::fstream::app | std::fstream::out);
				backup = std::cout.rdbuf();     // back up cout's streambuf
				psbuf = filestr.rdbuf();        // get file's streambuf
				std::cout.rdbuf(psbuf);         // assign streambuf to cout
				std::cout << str << std::endl;
				std::cout.rdbuf(backup);        // restore cout's original streambuf
				filestr.close();
			} else {
				str.append(" ======================================================\n");
				OutputDebugString(str.c_str());
				//std::cout << str << std::endl;
			}
		}

		class Odbc
		{
#define MAXBUFLEN   255
			//#define ROW_ARRAY_SIZE 100000

		private:
			//Nomad::Bio::MatcherFacade matcherFacade;
			Nomad::Bio::MatcherFacade	*matcherFacadePtr;

			//void *buffer;
			//void *buffer2;
			//bool matchingStatus;
			//typedef pair <int, HNImage> imageMapPair;
			//std::map <int, HNImage> imageMap;

		private:
			//void printStatusStatement(char * statusStatement);
			//void FreeStmtHandle(SQLHSTMT hStmt);
			//void extract_error(char *fn, SQLHANDLE handle, SQLSMALLINT type);
			void extract_error(char *fn, SQLHANDLE handle, SQLSMALLINT type, std::string *errorMessage);
			//void clean();
			//void TimedRun(Func test, string label);

			//public:

			SQLHENV hEnv;
			SQLHDBC hDBC;
			SQLHSTMT hStmt;
			//char ConnStrIn[MAXBUFLEN];
			SQLCHAR ConnStrIn[MAXBUFLEN];
			SQLCHAR ConnStrOut[MAXBUFLEN];
			SQLSMALLINT cbConnStrOut;
			SQLRETURN rc;

			std::string dbSettings[4];

		public:
			static bool	terminateLoop;
			static bool	fillOnly;

		public:
			Odbc(unsigned char *probeTemplate, unsigned __int32 probeTemplateSize, char* appSettings[]);		//probe template
			~Odbc();
			bool getRowCount(unsigned __int32 *rowcount, std::string *errorMessage);
			bool getAppId(unsigned __int32 *appid, std::string *errorMessage);
			unsigned __int32 exec(unsigned long int, unsigned int, char *fingerList[], __int32 fingerListSize, std::string *errorMessage, fnCallBack callBack);
			//static void enroll(unsigned char *record, unsigned __int32 size);
			//static void terminate();
			//void disconnect();

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
