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

		unsigned __int32 __stdcall match(unsigned char *record, unsigned __int32 size, char *errorMessage) {
			unsigned __int32 retcode = 0;

			std::cout << errorMessage << endl;

			odbcPtr = new Nomad::Data::Odbc();
			unsigned __int32 rowcount = 0;

			//bool retcode = false;
			//if (odbcPtr->getRowCount(&rowcount))
			//	retcode = true;
			if (!odbcPtr->getRowCount(&rowcount)) {
				delete odbcPtr;
				//errorMessage = "back";
				//errorMessage = reinterpret_cast<char*>("back");
				strcpy_s(errorMessage, 9, "kukuback");

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
