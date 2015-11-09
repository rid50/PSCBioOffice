#pragma once

#include "Nomad.Data.h"

using namespace std;

namespace Nomad {
	namespace Lookup {
		Nomad::Data::Odbc *odbcPtr;

		extern "C" 
		unsigned __int32 __declspec(dllexport) __stdcall match(char *arrOffingers[], __int32 arrOfFingersSize, 
														unsigned char *record, unsigned __int32 size, 
														char *errorMessage, __int32 messageSize);

		extern "C" void __declspec(dllexport) __stdcall terminateMatchingService();

		inline void printStatusStatement(double statusStatement) {
#ifdef _DEBUG
			std::cout << statusStatement << std::endl;
#endif
		}
	}
}