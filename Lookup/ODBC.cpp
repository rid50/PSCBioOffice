//#include "stdafx.h"
//#include <windows.h>
//#include <conio.h>
/*
#include "stdafx.h"
#include <sql.h>
#include <sqlext.h>
#include <sqltypes.h>
#include <iostream>
#include <conio.h>
*/
//#pragma once

//#include <NErrors.h>
//#include <NMatcher.h>

#include "Nomad.Data.h"
//#include "Utils.h"

#include <tchar.h>
#include <concrt.h>
#include <ppl.h>

using namespace ::Concurrency;
//using namespace std;

//#pragma comment(lib, "odbc32.lib")
//#pragma comment(lib, "user32.lib")
//using namespace std;

int _tmain(int argc, _TCHAR* argv[])
{
	//getLicenses();

	//Nomad::Bio::Matcher* matcher = new Nomad::Bio::Matcher();
	//Nomad::Bio::MatcherFacade matcherFacade;
	//matcherFacade.enroll();

	//matcher->enroll();

		Nomad::Data::Odbc odbc;
		int rowcount = 0;
		if (odbc.connect(&rowcount) == 0) {

			LARGE_INTEGER begin, end, freq;
			QueryPerformanceCounter(&begin);

			unsigned int limit = 10000;
			unsigned int topindex = rowcount/limit + 1;
			//topindex = 1;
			//limit = 5;
			//for (int k = 0; k < 100; k++) {
			if (0) {
				task_group tg;
				tg.run_and_wait([&] {
					parallel_for(0u, topindex, [&](unsigned int i) {
						if (odbc.exec((unsigned long int)(i * limit), limit) == -1)
							tg.cancel();
					});
				});
			} else {
				for (unsigned int i = 0; i < topindex; i++) {
					//if (odbc.exec(i * limit, i * limit + limit, limit) != 0)
					if (odbc.exec((unsigned long int)(i * limit), limit) == -1)
						break;
				}
			}
			//}
			QueryPerformanceCounter(&end);
			QueryPerformanceFrequency(&freq);

			double result = (end.QuadPart - begin.QuadPart) / (double) freq.QuadPart;
			printf("%s : %4.2f ms\n", "ODBC - Time elapsed: ", result * 1000);
		}

		//matcher.~Matcher();
		//delete matcher;

		std::cout<<"\nPress any key to exit";
		_getch();
			

		//std::cin.get();

		return 0;

}

//#define N_E_FAILED	-1
//#define NFailed(result) ((result) < 0)
//#define NSucceeded(result) ((result) >= 0)
/*
	void getLicenses() {
		//hExtractor = NULL;
		//hMatcher = NULL;

		NResult result;
		const NChar * LicensesMain[] = { "FingersExtractor", "FingersMatcher" };
		const NChar * LicensesBSS[] = { "FingersBSS" };

		result = obtainLicense(LicensesMain, sizeof(LicensesMain)/sizeof(LicensesMain[0]));
		if (NFailed(result))
		{
			if (result == N_E_FAILED)
			{
				printStatusStatement("Failed to obtain licenses for extractor and/or matcher");
			}
			else
			{
				printStatusStatement("Licensing manager is not running");
			}
		}
		else
		{
			printStatusStatement("Licenses for extractor and/or matcher obtained");
		}

		result = obtainLicense(LicensesBSS, sizeof(LicensesBSS)/sizeof(LicensesBSS[0]));
		if (NSucceeded(result))
		{
			printStatusStatement("Licenses for BSS obtained");
		}

		//result = NfeCreate(&hExtractor);

		//if (result != N_OK) {
		//	printStatusStatement("Error creating the Extractor");
		//	return;
		//}

		//result = NMCreate(&hMatcher);

		//if (result != N_OK) {
		//	printStatusStatement("Error creating the Matcher");
		//	return;
		//}
	}

	inline void printStatusStatement(char * statusStatement) {
		#ifdef _DEBUG
			//sprintf_s (Matcher::statusStatement, statusStatement);
			std::cout << statusStatement << std::endl;
		#endif
	}
*/