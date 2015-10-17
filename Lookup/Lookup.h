#pragma once

#include "Nomad.Data.h"
//#include <string>

using namespace std;

namespace Nomad {
	namespace Lookup {
		Nomad::Data::Odbc *odbcPtr;
		//int rowcount;

		//extern "C" bool __declspec(dllexport) __stdcall initFingerMatcher();
		extern "C" unsigned __int32 __declspec(dllexport) __stdcall match(unsigned char *record, unsigned __int32 size, char *errorMessage, __int32 messageSize);

		inline void printStatusStatement(double statusStatement) {
#ifdef _DEBUG
			//sprintf_s (ServiceListener::statusStatement, statusStatement);
			std::cout << statusStatement << std::endl;
#endif
		}
	}
}