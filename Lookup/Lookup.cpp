//#include "stdafx.h"
#include "Lookup.h"

//#include <cstdlib>
/*
#include <concrt.h>
#include <ppl.h>
//#include <pplawait.h>
#include <ppltasks.h>
//#include <vector>
*/
//#include <windows.h>
//#include "threadsafe_queue.cpp"

using namespace ::Concurrency;

//using namespace System::Collections::Concurrent;
//#using <mscorlib.dll>
//using namespace System;

//using namespace System::Runtime::Remoting;

namespace Nomad {
	task_group tg;

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
		//fnNotify _callBack;

		//fnCallBack _callBack;

		////void MySetCallBack(fnNotify callBack) {
		//void __stdcall SetCallBack(fnCallBack callBack) {
		//	_callBack = callBack;
		//}

		void __stdcall terminateMatchingService() {
			//int k = 0;
			//Nomad::Data::Odbc::terminateLoop = true;
			//Nomad::Data::Odbc::terminate(true);
			try {
				tg.cancel();
				_terminate = true;
			}
			catch (std::exception&) {}
		}

		void __stdcall fillCache(char *fingerList[], __int32 fingerListSize, char *appSettings[], fnCallBack callBack) {
			Nomad::Data::Odbc::fillOnly = true;
			__int32 messageSize = MESSAGE_SIZE;
			//char errorMessage[messageSize];
			char *errorMessage = new char[messageSize];
			errorMessage[0] = '\0';

			//CallBackStruct callBackParam;
			//callBackParam.code = 22;
			////callBackParam.text = L"kuku";
			//wcscpy_s(callBackParam.text, 5, L"kuku");

			////strcpy_s(callBackParam.text, 5, "kuku" + '\0');
			//_callBack(&callBackParam);
				
			run(fingerList, fingerListSize, NULL, 0, appSettings, errorMessage, messageSize, callBack);

			if (strlen(errorMessage) != 0) {
				CallBackStruct callBackParam;
				callBackParam.code = 3;
				size_t length = strlen(errorMessage);
				mbstowcs_s(&length, callBackParam.text, errorMessage, length);
				//wcscpy_s(callBackParam.text, messageSize, static_cast<wchar_t>(errorMessage));
				callBack(&callBackParam);
			}

			delete[] errorMessage;
		}


		//std::shared_ptr<T> wait_and_pop()
		task<std::shared_ptr<int>> getQueueItemAsync(BlockingCollection<int>& bc)
		{
			return create_task([&bc]() {
				return bc.wait_and_pop();
			});
		}

		//task<void> process_queue_async(task<void> process_queue) __resumable {
		//	return process_queue;
		//	__await process_queue;
		//	return create_task([]() { return; });
		//}

		//task<void> process_queue(fnCallBack callBack, BlockingCollection<int>& bc, int topindex)
		//task<void> process_blocking_queue(fnCallBack callBack, BlockingCollection<int>*bc, int topindex, task_group &tg)
		void process_blocking_queue(fnCallBack callBack, BlockingCollection<int>*bc, int topindex, task_group &tg)
		{
			//return create_task([&]() {
				shared_ptr<int> i;
				CallBackStruct callBackParam;
				string temp;
				char const* chars;
				while (true)
				{
					//if (Nomad::Data::Odbc::terminateLoop)
					//	break;
					if (tg.is_canceling())
						break;

					//shared_ptr<int> i = 0;
					i = bc->wait_and_pop();
					//shared_ptr<int> i = getQueueItemAsync(bc).get();
					if (*i.get() == 0)
						break;

					//if ((*i.get() == -1 && --topindex == 0) || *i.get() == -2)
					if ((*i.get() == -1 && --topindex == 0))
						break;

					//Concurrency::wait(2);
					//CallBackStruct callBackParam;



					callBackParam.code = 1;


					//std::stringstream strm;
					//strm << NumRowsFetched;
					temp = to_string(*i.get());
					chars = temp.c_str();

					size_t length = strlen(chars);

					//size_t length = strlen("100");

					mbstowcs_s(&length, callBackParam.text, chars, length);

					//mbstowcs_s(&length, callBackParam.text, "100", length);
					//wcscpy_s(callBackParam.text, messageSize, static_cast<wchar_t>(errorMessage));
					callBack(&callBackParam);
				}
			//});
		}

		unsigned __int32 __stdcall match(char *fingerList[], __int32 fingerListSize,
			unsigned char *probeTemplate, unsigned __int32 probeTemplateSize, char *appSettings[], char *errorMessage, __int32 messageSize) {
				Nomad::Data::Odbc::fillOnly = false;
				return run(fingerList, fingerListSize, probeTemplate, probeTemplateSize, appSettings, errorMessage, messageSize, NULL);
		}

		unsigned __int32 run(char *fingerList[], __int32 fingerListSize,
			unsigned char *probeTemplate, unsigned __int32 probeTemplateSize, char *appSettings[], char *errorMessage, __int32 messageSize, fnCallBack callBack) {

			unsigned __int32 retcode = 0;

			LARGE_INTEGER begin, end, freq;
			QueryPerformanceCounter(&begin);

			_terminate = false;
			//std::stringstream ss2; 
			//ss2 << "1------------------------------------:";
			//Data::Log(ss2.str(), true);

			//std::cout << errorMessage << endl;
			try {
				odbcPtr = new Nomad::Data::Odbc(NULL, NULL, NULL, appSettings, NULL);
			} catch (std::exception&) {
				//const char *errMessage = e.what();
				const char *errMessage = "Cannot connect to a database, check a connection string";
				if (static_cast<unsigned __int32>(messageSize) < strlen(errMessage) + 1) {
					char *pchar = const_cast<char *>(errMessage);
				//if (static_cast<unsigned __int32>(messageSize) < strlen("Cannot connect to database: check a connection string") + 1) {
					//char *pchar = const_cast<char *>("Cannot connect to database: check a connection string");
					pchar[messageSize - 1] = '\0';
					strcpy_s(errorMessage, messageSize, pchar);
				} else
					strcpy_s(errorMessage, strlen(errMessage) + 1, errMessage);

				return 0;
			}

			//ss2.clear();
			//ss2 << "2------------------------------------:";
			//Data::Log(ss2.str(), true);

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

			if (callBack != NULL) {
				CallBackStruct callBackParam;
				callBackParam.code = 1;

				stringstream strm;
				strm << rowcount;
				string temp = strm.str();
				char const* chars = temp.c_str();

				size_t length = strlen(chars);
				mbstowcs_s(&length, callBackParam.text, chars, length);
				//wcscpy_s(callBackParam.text, messageSize, static_cast<wchar_t>(errorMessage));
				callBack(&callBackParam);
			}
			//return retcode;
			//odbcPtr->enroll(record, size);
			//Nomad::Data::Odbc::terminate(false);
			//Nomad::Data::Odbc::terminateLoop = false;
			//Nomad::Data::Odbc::enroll(record, size);

			BlockingCollection<int> bc;
			//threadsafe_queue<int> bc;

			//LARGE_INTEGER begin, end, freq;
			//QueryPerformanceCounter(&begin);

			unsigned int limit = BUFFERLEN;
			unsigned int topindex = rowcount/limit + 1;
			//topindex = 1;
			//limit = 5;
			//for (int k = 0; k < 100; k++) {
//			vector<int> results;

			//task<void> t;
			//if (&bc != NULL && callBack != NULL) {
			//	//process_queue(callBack, bc, topindex, tg);
			//	t = create_task([&]() {
			//		process_queue(callBack, bc, topindex, tg);
			//	});
			//}

			if (1) {
				//if (callBack != NULL)
				//	process_blocking_queue(callBack, &bc, topindex, tg);

				//process_queue_async(process_queue(callBack, bc, topindex));
				//task_group tg;
				//tg.run_and_wait([&] {
				tg.run([&] {
					parallel_for(0u, topindex, [&](size_t i) {
						//if (!Nomad::Data::terminateLoop) {
						//if (!tg.is_canceling()) {
							unsigned __int32 ret = 0;
							Nomad::Data::Odbc *odbcPtr = NULL;
							//Nomad::Data::Odbc *odbcPtr = new Nomad::Data::Odbc(probeTemplate, probeTemplateSize, appSettings);
							//if ((ret = odbcPtr->exec((unsigned long int)(i * limit), limit, &errMessage)) > 0) {
							try {
								odbcPtr = new Nomad::Data::Odbc(&bc, probeTemplate, probeTemplateSize, appSettings, &tg);
								ret = odbcPtr->exec((unsigned long int)(i * limit), limit, fingerList, fingerListSize, &errMessage, NULL);
							} catch (std::exception& e) {
								errMessage = "Error: ";
								errMessage += e.what();
								ret = 0;
								tg.cancel();

								//if (&bc != NULL)
								//	bc.push(-2);
							}

							if (ret > 0) {
								retcode = ret;
								//Nomad::Data::terminateLoop = true;
								//Nomad::Data::Odbc::terminate();
								tg.cancel();
							} else if (ret == 0 && errMessage.length() != 0) {
								retcode = 0;
								//Nomad::Data::terminateLoop = true;
								//Nomad::Data::Odbc::terminate();
								if (!tg.is_canceling())
									tg.cancel();

								//if (&bc != NULL)
								//	bc.push(-2);
							}

							if (odbcPtr != NULL) 
								delete odbcPtr;
						//}
						//else {
						//	tg.cancel();
						//}
					});

//					Nomad::Data::Odbc::terminate(true);

					//if (callBack != NULL)
					//	process_queue(callBack, bc, topindex, tg);
					//process_queue_async(process_queue(callBack, bc, topindex)).get();

				});

				if (callBack != NULL)
					process_blocking_queue(callBack, &bc, topindex, tg);

				//if (&bc != NULL)
				//	process_queue(callBack, bc, topindex, tg);

				tg.wait();
			} else {
				Nomad::Data::Odbc *odbcPtr = new Nomad::Data::Odbc(NULL, probeTemplate, probeTemplateSize, appSettings, &tg);
				for (unsigned int i = 0; i < topindex; i++) {
					//if (odbc.exec(i * limit, i * limit + limit, limit) != 0)
					//if ((retcode = odbcPtr->exec((unsigned long int)(i * limit), limit, fingerList, fingerListSize, &errMessage)) > 0) {
					try {
						retcode = odbcPtr->exec((unsigned long int)(i * limit), limit, fingerList, fingerListSize, &errMessage, callBack);
						//retcode = odbcPtr->exec((unsigned long int)(i * limit + 80000), limit, fingerList, fingerListSize, &errMessage, _callBack);
					} catch (std::exception& e) {
						errMessage = "Error: ";
						errMessage += e.what();
						retcode = 0;
					}

					if (retcode > 0) {
						break;
					} else if (retcode == 0 && errMessage.length() != 0) {
						break;
					}
				}
				delete odbcPtr;
			}
			
			//if (&bc != NULL)
			//	bc.push(-1);


			//if (&bc != NULL && !tg.is_canceling()) {
			//	while (!bc.is_empty()) {}
			//	bc.push(0);
			//	//while (!bc.is_empty()) {}
			//}

			//t.get();

//#ifdef _DEBUG
			if (callBack != NULL && !_terminate) {
				QueryPerformanceCounter(&end);
				QueryPerformanceFrequency(&freq);

				double result = (end.QuadPart - begin.QuadPart) / (double) freq.QuadPart;

				//printf("%s : %4.2f ms\n", "ODBC - Time elapsed: ", result * 1000);

				char buffer [90];
				sprintf_s(buffer, 90, "--- Time elapsed: %4.2f min", result / 60);
				//sprintf(buffer, "%s : %4.2f min", "--- Time elapsed: ", result / 60);

				//Concurrency::wait(10);

				CallBackStruct callBackParam;
				callBackParam.code = 2;
				size_t length = strlen(buffer);
				mbstowcs_s(&length, callBackParam.text, buffer, length);
				//wcscpy_s(callBackParam.text, messageSize, static_cast<wchar_t>(errorMessage));
				callBack(&callBackParam);
			}

			if (_terminate) {
				errMessage = "The request was cancelled";
				retcode = 0;
			}

			//printf ("%s",buffer);

			//printStatusStatement(result);
			//std::cout << result << " sec" << endl;

			//std::stringstream ss; 
			//ss << result << " sec";
			//Data::Log(ss.str(), false);
//#endif
			//OutputDebugString(ss.str().c_str());

			if (retcode > 0) {
				retcode--;
				odbcPtr = new Nomad::Data::Odbc(NULL, NULL, NULL, appSettings, NULL);
				//std::string errMessage;
				if (!odbcPtr->getAppId(&retcode, &errMessage))
					retcode = 0;
				delete odbcPtr;
			} 
				
			if (retcode == 0 && errMessage.length() != 0) {
				if (static_cast<unsigned __int32>(messageSize) < errMessage.length() + 1)
					strcpy_s(errorMessage, messageSize, errMessage.substr(0, messageSize - 1).c_str() + '\0');
				else
					strcpy_s(errorMessage, errMessage.length() + 1, errMessage.c_str());
			}

			if (callBack != NULL) {
				CallBackStruct callBackParam;
				callBackParam.code = 0;
				callBack(&callBackParam);
			}

			if (&bc != NULL) {
				bc.empty();
			}

			return retcode;
		}
	}
}
