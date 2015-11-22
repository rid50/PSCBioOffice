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
#include <iostream>     // std::streambuf, std::cout
#include <memory>
#include <map>
#include <sstream>		// std::stringstream

//#include <NFExtractor.h>
//#include <NMatcher.h>
//#include <NImage.h>
//#include <NImageFile.h>

#include <Core/NObject.h>
#include <NLicensing.h>
#include <NBiometricClient.h>
#include <NBiometrics.h>
#include <NMedia.h>
#include <NCore.h>

//#include <NBiometricClient.hpp>
//#include <NGui.hpp>
//#include <NBiometricGui.hpp>

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
			//Nomad::Bio::Matcher* matcherPtr;
		public:
			MatcherFacade();
			~MatcherFacade();
			void enroll(unsigned char *probeTemplate, unsigned __int32 probeTemplateSize);
			bool match(void *galleryTemplate, unsigned __int32 galleryTemplateSize);
		};

			NResult		result;
			void LicenseRelease();
			void LicenseObtain();

		class License
		{
		private:
			NResult		result;
		public:
			License();
			~License();
			inline void printStatusStatement(const char * statusStatement) {
#ifdef _DEBUG
				//sprintf_s (Matcher::statusStatement, statusStatement);
				std::cout << statusStatement << std::endl;
#endif
			}
		};

		class Matcher
		{
		private:
			NResult				result;
			HNBiometricClient	hBiometricClient;

			HNSubject	hProbeSubject;
			HNSubject	hGallerySubject;

			//static NSizeType	enrolledTemplateSize;
			//static void			*enrolledTemplate;
			//typedef pair <int, HNImage> imageMapPair;
			//std::map <int, HNImage> imageMap;

		public:
			Matcher();
			~Matcher();
			//bool readImages(wchar_t * szFileName, wchar_t * szFileName2);
			//int extract(HNImage nImage, NfeExtractionStatus* extractionStatus, HNFRecord* hRecord);
			void enroll(unsigned char *enrolledTemplate, unsigned __int32 enrolledTemplateSize);
			bool match(void *prescannedTemplate, unsigned __int32 prescannedTemplateSize);
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

			inline void printStatusStatement(const char * statusStatement) {
#ifdef _DEBUG
				//sprintf_s (Matcher::statusStatement, statusStatement);
				std::cout << statusStatement << std::endl;
#endif
			}

			inline void printStatusStatement(const char * statusStatement, NResult result) {
#ifdef _DEBUG
				std::stringstream stmt;
				stmt << statusStatement << result;
				printStatusStatement(stmt.str().c_str());
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
