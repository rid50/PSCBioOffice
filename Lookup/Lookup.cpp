#include "Lookup.h"

//#include <cstdlib>

#include <concrt.h>
#include <ppl.h>

using namespace ::Concurrency;

//using namespace System;
//using namespace System::Runtime::Remoting;

namespace Nomad {
	namespace Lookup {
		bool __stdcall init() {
			odbcPtr = new Nomad::Data::Odbc();
			rowcount = 0;
			if (odbcPtr->connect(&rowcount) == 0)
				return true;
			else
				return false;
		}

		unsigned __int32 __stdcall match(unsigned char *record, unsigned __int32 size) {
			unsigned __int32 retcode = 0;

			odbcPtr->enroll(record, size);

			LARGE_INTEGER begin, end, freq;
			QueryPerformanceCounter(&begin);

			unsigned int limit = 10000;
			unsigned int topindex = rowcount/limit + 1;
			//topindex = 1;
			//limit = 5;
			//for (int k = 0; k < 100; k++) {
			if (1) {
				task_group tg;
				tg.run_and_wait([&] {
					parallel_for(0u, topindex, [&](unsigned int i) {
						if ((retcode = odbcPtr->exec((unsigned long int)(i * limit), limit)) == -1)
							tg.cancel();
					});
				});
			} else {
				for (unsigned int i = 0; i < topindex; i++) {
					//if (odbc.exec(i * limit, i * limit + limit, limit) != 0)
					if ((retcode = odbcPtr->exec((unsigned long int)(i * limit), limit)) == -1)
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

			std::cout << result << " sec" << endl;

			return retcode;
		}
	}
}
