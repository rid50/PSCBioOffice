#pragma once

#include "Nomad.Data.h"
#include <string>

using namespace std;

namespace Nomad {
	namespace Lookup {
		Nomad::Data::Odbc *odbcPtr;
		int rowcount;

		extern "C" bool __declspec(dllexport) __stdcall init();
		extern "C" unsigned __int32 __declspec(dllexport) __stdcall match(unsigned char *record, unsigned __int32 size);
	}
}