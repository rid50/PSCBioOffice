#include "Lookup.h"

//#include <cstdlib>

#include <concrt.h>
#include <ppl.h>
//#include <vector>

using namespace ::Concurrency;

//using namespace System;
//using namespace System::Runtime::Remoting;

namespace Nomad {
	namespace Lookup {
		//bool __stdcall initFingerMatcher() {
		//	//odbcPtr = new Nomad::Data::Odbc();
		//	//rowcount = 0;
		//	//bool retcode = false;
		//	//if (odbcPtr->getRowCount(&rowcount))
		//	//	retcode = true;

		//	//delete odbcPtr;
		//	//return retcode;
		//	return true;
		//}

		unsigned __int32 __stdcall match(unsigned char *record, unsigned __int32 size, char *errorMessage, __int32 messageSize) {
			unsigned __int32 retcode = 0;

			//std::cout << errorMessage << endl;

			odbcPtr = new Nomad::Data::Odbc();
			unsigned __int32 rowcount = 0;

			//bool retcode = false;
			//if (odbcPtr->getRowCount(&rowcount))
			//	retcode = true;
			std::string errMessage;
			if (!odbcPtr->getRowCount(&rowcount, &errMessage)) {
				delete odbcPtr;

				//strcpy_s(errorMessage, strlen(str.c_str()) + 1, str.c_str());
				//if (
				//strcpy_s(errorMessage, str.length() + 1, str.c_str());

				//errorMessage = "back";
				//errorMessage = reinterpret_cast<char*>("back");
				//unsigned int i = static_cast<unsigned __int32>(messageSize);
				//bool b = static_cast<unsigned __int32>(messageSize) < errMessage.length() + 1;
				if (static_cast<unsigned __int32>(messageSize) < errMessage.length() + 1)
					strcpy_s(errorMessage, messageSize, errMessage.substr(0, messageSize - 1).c_str() + '\0');
				else
					strcpy_s(errorMessage, errMessage.length() + 1, errMessage.c_str());

				//strcpy_s(errorMessage, static_cast<unsigned __int32>(messageSize) < errMessage.length() + 1 ? messageSize - 1 : errMessage.length() + 1, errMessage.c_str());

				//std:string str = "bask";
				//errorMessage = const_cast<char*>(str.c_str());
				return 0;
			}
			
			delete odbcPtr;
			//return retcode;
			//odbcPtr->enroll(record, size);
			Nomad::Data::Odbc::terminateLoop(false);
			Nomad::Data::Odbc::enroll(record, size);

			LARGE_INTEGER begin, end, freq;
			QueryPerformanceCounter(&begin);

			unsigned int limit = 10000;
			unsigned int topindex = rowcount/limit + 1;
			//topindex = 1;
			//limit = 5;
			//for (int k = 0; k < 100; k++) {
//			vector<int> results;
			if (1) {
				task_group tg;
				tg.run_and_wait([&] {
					parallel_for(0u, topindex, [&](size_t i) {
						unsigned __int32 ret = 0;
						Nomad::Data::Odbc *odbcPtr = new Nomad::Data::Odbc();
						if ((ret = odbcPtr->exec((unsigned long int)(i * limit), limit)) >= 0) {
							if (ret > 0) {
								retcode = ret;
								Nomad::Data::Odbc::terminateLoop(true);
							}
							tg.cancel();
						}
						delete odbcPtr;
					});
				});
			} else {
				for (unsigned int i = 0; i < topindex; i++) {
					//if (odbc.exec(i * limit, i * limit + limit, limit) != 0)
					if ((retcode = odbcPtr->exec((unsigned long int)(i * limit), limit)) > 0)
						break;
				}
			}
			//}
			QueryPerformanceCounter(&end);
			QueryPerformanceFrequency(&freq);

			double result = (end.QuadPart - begin.QuadPart) / (double) freq.QuadPart;
			//printf("%s : %4.2f ms\n", "ODBC - Time elapsed: ", result * 1000);

			//char buffer [30];
			//sprintf (buffer, "%s : %4.2f ms\n", "ODBC - Time elapsed: ", result * 1000);
			//printf ("%s",buffer);

			printStatusStatement(result);
			//std::cout << result << " sec" << endl;

			return retcode;
		}
	}
}
