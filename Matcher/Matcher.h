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
#include <stdio.h>
#include <tchar.h>
#include <malloc.h>

#include <string>
#include <iostream>
#include <memory>
#include <map>

#include <NFExtractor.h>
#include <NMatcher.h>
#include <NImage.h>
#include <NImageFile.h>

//#include <mutex>
//#include <thread>

//class IMatcher
//{
//public:
//	 virtual void enroll(void) = 0;
//	 virtual void match(void *buffer2, int size) = 0;
//};

//class __declspec(dllexport) Matcher

//HNFExtractor hExtractor;
//HNMatcher	hMatcher;
//void getLicenses();
//inline void printStatusStatement(char * statusStatement);

namespace Nomad
{
	namespace Bio
	{
		class __declspec(dllexport) MatcherFacade
		{
		public:
			void* matcherPtr;
		public:
			MatcherFacade();
			~MatcherFacade();
			static void enroll(unsigned char *record, unsigned __int32 size);
			bool match(void *prescannedTemplate, int prescannedTemplateSize);
		};

		class License
		{
		private:
			NResult		result;
		public:
			License();
			//~License();

			inline void printStatusStatement(char * statusStatement) {
#ifdef _DEBUG
				//sprintf_s (Matcher::statusStatement, statusStatement);
				std::cout << statusStatement << std::endl;
#endif
			}
		};

		class Matcher
		{
			//private:

		private:
			NResult		result;
			//HNFExtractor hExtractor;
			HNMatcher	hMatcher;
			//HNImageFile hImageFile;
			//HNImageFile hImageFile2;
			//HNImage		hImage;
			//HNImage		hImage2;
			//HNFRecord	hRecord;
			//HNFRecord	hRecord2;
			static NSizeType	enrolledTemplateSize;
			static void			*enrolledTemplate;

			//void		*buffer2;
			//bool		matchingStatus;
			//typedef pair <int, HNImage> imageMapPair;
			//std::map <int, HNImage> imageMap;

		private:
			//void checkExtractionStatus(const NfeExtractionStatus &extractionStatus);
			//void printStatusStatement(char * statusStatement);
			//void clean();
			//void TimedRun(Func test, string label);

		public:
			Matcher();
			~Matcher();
			//bool readImages(wchar_t * szFileName, wchar_t * szFileName2);
			//int extract(HNImage nImage, NfeExtractionStatus* extractionStatus, HNFRecord* hRecord);
			static void enroll(unsigned char *record, unsigned __int32 size);
			bool match(void *prescannedTemplate, int prescannedTemplateSize);
			//void __declspec(dllexport) enroll(void);
			//void __declspec(dllexport) match(void *buffer2, int size);
			//void run();
			//bool getMatchingStatus();

		//private:
			/// <summary>
			/// Executes a function and prints timing results
			/// </summary>
			//template<typename Func>
			//void TimedRun(Func test, std::string label)
			//{
			//	LARGE_INTEGER begin, end, freq;
			//	QueryPerformanceCounter(&begin);

			//	// invoke the function
			//	test();

			//	// print timings
			//	QueryPerformanceCounter(&end);

			//	QueryPerformanceFrequency(&freq);

			//	double result = (end.QuadPart - begin.QuadPart) / (double) freq.QuadPart;
			//	printf("%s : %4.2f ms\n", label.c_str(), result * 1000);

			//	//return result;
			//}

			inline void printStatusStatement(char * statusStatement) {
#ifdef _DEBUG
				//sprintf_s (Matcher::statusStatement, statusStatement);
				std::cout << statusStatement << std::endl;
#endif
			}

			//inline NInt NCheck(NResult result)
			//{
			//	if(NFailed(result))
			//	{
			//		//cout << "-- " + result << std::endl;

			//		//NRaiseError(result);
			//	}
			//	return result;
			//}
		};
	}
}
#endif
