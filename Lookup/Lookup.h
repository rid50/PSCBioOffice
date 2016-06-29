#pragma once
#ifndef LOOKUP_H
#define LOOKUP_H

#include <concrt.h>
#include <ppl.h>
//#include <pplawait.h>
#include <ppltasks.h>

#include "Nomad.Data.h"

using namespace std;

namespace Nomad {

	//extern "C" Concurrency::task_group tg;
	//Concurrency::task_group tg;

	namespace Lookup {
		Nomad::Data::Odbc *odbcPtr;

		bool _terminate;

		//typedef struct _tagCallBackStruct
		//{
		//	short code;
		//	wchar_t text[MESSAGE_SIZE]; 
		//} CallBackStruct;

		////typedef void (__stdcall *fnNotify)(int notifyCode);
		////typedef void (__stdcall *fnCallBack)(int callBackParam);
		//typedef void (__stdcall *fnCallBack)(CallBackStruct* callBackParam);
		//typedef void (__stdcall *fnNotify)(int notifyCode, NotifyStruct* notifyInfo);
		//extern "C" void __declspec(dllexport) __stdcall MySetCallBack(fnNotify callBack);

		//extern "C" void __declspec(dllexport) __stdcall SetCallBack(fnCallBack callBack);

		extern "C"
		void __declspec(dllexport) __stdcall fillCache(char *fingerList[], __int32 fingerListSize, char *appSettings[], fnCallBack callBack);

		extern "C"
		unsigned __int32 __declspec(dllexport) __stdcall match(char *fingerList[], __int32 fingerListSize, 
														unsigned char *record, unsigned __int32 size,
														char *appSettings[],
														char *errorMessage, __int32 messageSize);

		extern "C" void __declspec(dllexport) __stdcall terminateMatchingService();

		unsigned __int32 run(char *fingerList[], __int32 fingerListSize, 
														unsigned char *record, unsigned __int32 size,
														char *appSettings[],
														char *errorMessage, __int32 messageSize, fnCallBack callBack);

		inline void printStatusStatement(double statusStatement) {
#ifdef _DEBUG
			std::cout << statusStatement << std::endl;
#endif
		}
	}
}
#endif