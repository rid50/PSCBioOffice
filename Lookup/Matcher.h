//#include "stdafx.h"

#ifndef SERVICE_LISTENER_H
#define SERVICE_LISTENER_H
//#include <NCore.h>
//#include <NLicensing.h>

//#include "stdafx.h"
//#include <WinDef.h>
//#include <vector>
//#include <malloc.h>
//#include <iostream>
//#include <memory>

#include <Windows.h>
#include <string>

#include <NFExtractor.h>
#include <NMatcher.h>

namespace Nomad
{

	class Matcher
	{
		//private:

		private:
			NResult result;
			HNFExtractor hExtractor;
			HNMatcher	hMatcher;
			HNImageFile hImageFile;
			HNImageFile hImageFile2;
			HNImage hImage;
			HNImage hImage2;
			HNFRecord hRecord;
			HNFRecord hRecord2;
			void *buffer;
			void *buffer2;
			bool matchingStatus;
			//typedef pair <int, HNImage> imageMapPair;
			std::map <int, HNImage> imageMap;

		private:
			void checkExtractionStatus(const NfeExtractionStatus &extractionStatus);
			//void printStatusStatement(char * statusStatement);
			void clean();
			//void TimedRun(Func test, string label);

		public:
			Matcher();
			~Matcher();
			bool readImages(wchar_t * szFileName, wchar_t * szFileName2);
			void run();
			bool getMatchingStatus();


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
					//sprintf_s (Matcher::statusStatement, statusStatement);
					std::cout << statusStatement << std::endl;
				#endif
			}

			inline NInt NCheck(NResult result)
			{
				if(NFailed(result))
				{
					//cout << "-- " + result << std::endl;

					//NRaiseError(result);
				}
				return result;
			}
	};
}

#endif
