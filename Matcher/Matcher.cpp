//#include "stdafx.h"

//#include "Utils.h"
#include "Matcher.h"

//using namespace Neurotec;
//using namespace Neurotec::Licensing;

//using namespace std;

BOOL APIENTRY DllMain(HANDLE hModule, 
                    DWORD  ul_reason_for_call, 
                    LPVOID lpReserved)
{
	switch( ul_reason_for_call ) {
		case DLL_THREAD_ATTACH:
		case DLL_THREAD_DETACH:
			break;
		case DLL_PROCESS_ATTACH:
			//Nomad::Bio::LicenseObtain();
			break;
		case DLL_PROCESS_DETACH:
			//Nomad::Bio::LicenseRelease();
			break;
	}
	return TRUE;
}


//inline void printStatusStatement(char * statusStatement) {
//	#ifdef _DEBUG
//		//sprintf_s (Matcher::statusStatement, statusStatement);
//		std::cout << statusStatement << std::endl;
//	#endif
//}

namespace Nomad
{
	namespace Bio
	{
		MatcherFacade::~MatcherFacade() {
			delete static_cast<Nomad::Bio::Matcher*>(matcherPtr);
		};

		MatcherFacade::MatcherFacade() {
			//Nomad::Bio::Matcher* matcherPtr = new Nomad::Bio::Matcher();
			matcherPtr = new Nomad::Bio::Matcher();
		};

		void MatcherFacade::enroll(unsigned char *probeTemplate, unsigned __int32 probeTemplateSize) {
			//static_cast<Nomad::Bio::Matcher*>(matcherPtr)->enroll(record, size);
			static_cast<Nomad::Bio::Matcher*>(matcherPtr)->enroll(probeTemplate, probeTemplateSize);
		}

		bool MatcherFacade::match(void *galleryTemplate, unsigned __int32 galleryTemplateSize) {
			return static_cast<Nomad::Bio::Matcher*>(matcherPtr)->match(galleryTemplate, galleryTemplateSize);
		}

		const NChar * components = N_T("Biometrics.FingerExtraction,Biometrics.FingerMatching");

		//License::License() {
		void LicenseObtain() {
			NBool available = NFalse;

			result = NLicenseObtainComponents(N_T("/local"), N_T("5000"), components, &available);
			if (NFailed(result))
			{ 
				char *str = "Biometrics: Failed to obtain licenses";
				//printStatusStatement(str);
				throw std::runtime_error(str);
			}

			if (!available)
			{
				std::stringstream stmt;
				stmt << "Biometrics: Licenses for " << components << " are not available";
				//printStatusStatement(stmt.str().c_str());
				throw std::runtime_error(stmt.str().c_str());
			}
			OutputDebugString(" ************************  License constructor ************************************** ");

		}

		//License::~License() {
		void LicenseRelease() {
			OutputDebugString(" ************************  License ************************************** ");
			result = NLicenseReleaseComponents(components);
			OutputDebugString(" ************************  License2 ************************************** ");
			if (NFailed(result))
			{
			OutputDebugString(" ************************  License3 ************************************** ");
				//printStatusStatement("Error releasing license components");
				throw std::runtime_error("Biometrics: Error releasing license components");
			}
			OutputDebugString(" ************************  License4 ************************************** ");

		}

		//NSizeType	Matcher::enrolledTemplateSize = 0;
		//void		*Matcher::enrolledTemplate = 0;
		//HNSubject	Matcher::hProbeSubject = NULL;

		Matcher::~Matcher() noexcept(false) {
			result = NObjectSet(NULL, (HNObject *)&hProbeSubject);
			if (NFailed(result))
			{
				//printStatusStatement("Error clearing resources");
				throw std::runtime_error("Biometrics: Error deleting probe subject");
			}

			result = NObjectSet(NULL, (HNObject *)&hBiometricClient);
			if (NFailed(result))
			{
				//printStatusStatement("Error clearing resources");
				throw std::runtime_error("Biometrics: Error deleting BiometricClient");
			}
		};

		//License license;

		Matcher::Matcher() {
			hBiometricClient = NULL;
			NInt				matchingThreshold = 40;
			NMatchingSpeed		matchingSpeed =	nmsLow;
			//NMatchingSpeed matchingSpeed = nmsHigh;

			//License license;

			// create biometric client
			result = NBiometricClientCreate(&hBiometricClient);
			if (NFailed(result))
			{
				//printStatusStatement("Biometric client creation failed:", result);
				throw std::runtime_error("Biometric client creation failed");
			}
			
			// set matching threshold
			result = NObjectSetPropertyP(hBiometricClient, N_T("Matching.Threshold"), N_TYPE_OF(NInt32), naNone, &matchingThreshold, sizeof(matchingThreshold), 1, NTrue);
			if (NFailed(result))
			{
				//printStatusStatement("Error setting matching threshold:", result);
				throw std::runtime_error("Biometrics: Error setting matching threshold");
			}

			// set matching speed
			result = NObjectSetPropertyP(hBiometricClient, N_T("Fingers.MatchingSpeed"), N_TYPE_OF(NMatchingSpeed), naNone, &matchingSpeed, sizeof(matchingSpeed), 1, NTrue);
			if (NFailed(result))
			{
				//printStatusStatement("Error setting matching speed:", result);
				throw std::runtime_error("Biometrics: Error setting matching speed");
			}
		}

		void Matcher::enroll(unsigned char *probeTemplate, unsigned __int32 probeTemplateSize) {
			HNString hSubjectId = NULL;
			
			//enrolledTemplate = record;
			//enrolledTemplateSize = size;

			hProbeSubject = NULL;
			NResult	result;
			NSizeType pSize = 0;
			result = NSubjectCreateFromMemory(probeTemplate, (NSizeType)probeTemplateSize, 0, &pSize, &hProbeSubject);
			if (NFailed(result))
			{
				throw std::runtime_error("Biometrics: Error creating a subject for enrolled template");
			}

			//// create probe subject id
			//result = NStringCreate(N_T("ProbeSubject"), &hSubjectId);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error creating subject id");
			//}

			//// set the id for the subject
			//result = NSubjectSetIdN(hProbeSubject, hSubjectId);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error setting subject id");
			//}

			//// free unneeded hSubjectId
			//result = NStringSet(NULL, &hSubjectId);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error deleting subject id");
			//}
		}

		bool Matcher::match(void *galleryTemplate, unsigned __int32 galleryTemplateSize) {
			NResult				result;
			//HNBiometricClient	hBiometricClient;

			//hBiometricClient = NULL;
			//NInt				matchingThreshold = 40;
			//NMatchingSpeed		matchingSpeed =	nmsLow;

			//HNString hSubjectId = NULL;
			//HNString hMatchId = NULL;

			NInt matchScore = 0;
			//NInt resultsCount = 0;

			//HNMatchingResult * hMatchingResults = NULL;
			HNMatchingResult	hMatchingResult;

			NBiometricStatus biometricStatus = nbsNone;
			//HNString hBiometricStatus = NULL;
			//const NChar * szBiometricStatus = NULL;

			//HNBiometricTask hBiometricTask = NULL;
			hGallerySubject = NULL;

			//NMMatchDetails **ppMatchDetails = NULL;
			//NSizeType packedSize2 = prescannedTemplateSize;
			//try {

			// create biometric client
			//result = NBiometricClientCreate(&hBiometricClient);
			//if (NFailed(result))
			//{
			//	//printStatusStatement("Biometric client creation failed:", result);
			//	throw std::runtime_error("Biometric client creation failed");
			//}
			//
			//// set matching threshold
			//result = NObjectSetPropertyP(hBiometricClient, N_T("Matching.Threshold"), N_TYPE_OF(NInt32), naNone, &matchingThreshold, sizeof(matchingThreshold), 1, NTrue);
			//if (NFailed(result))
			//{
			//	//printStatusStatement("Error setting matching threshold:", result);
			//	throw std::runtime_error("Biometrics: Error setting matching threshold");
			//}

			//// set matching speed
			//result = NObjectSetPropertyP(hBiometricClient, N_T("Fingers.MatchingSpeed"), N_TYPE_OF(NMatchingSpeed), naNone, &matchingSpeed, sizeof(matchingSpeed), 1, NTrue);
			//if (NFailed(result))
			//{
			//	//printStatusStatement("Error setting matching speed:", result);
			//	throw std::runtime_error("Biometrics: Error setting matching speed");
			//}

			//// create biometric task to enroll
			//result = NBiometricEngineCreateTask(hBiometricClient, nboEnroll, NULL, NULL, &hBiometricTask);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error creating a task");
			//}

			NSizeType pSize = 0;
			result = NSubjectCreateFromMemory(galleryTemplate, (NSizeType)galleryTemplateSize, 0, &pSize, &hGallerySubject);
			if (NFailed(result))
			{
				//printStatusStatement("Error creating a subject for prescanned template:", result);
				throw std::runtime_error("Biometrics: Error creating a subject for gallery template");
			} 

			//// create gallery subject id
			//result = NStringFormat(&hSubjectId, N_T("GallerySubject_{I32}"), 0);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error creating gallery id");
			//}

			//// set the id for the gallery subject
			//result = NSubjectSetIdN(hGallerySubject, hSubjectId);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error setting gallery id");
			//}

			//// add gallery subject to biometric task
			//result = NBiometricTaskAddSubject(hBiometricTask, hGallerySubject, NULL);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error adding a gallery subject to task");
			//}

			//// free unneeded hGallerySubject
			//result = NObjectSet(NULL, (HNObject *)&hGallerySubject);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error deleting gallery subject");
			//}

			//// free unneeded hSubjectId
			//result = NStringSet(NULL, &hSubjectId);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error deleting gallery subject id");
			//}

			//// perform biometric task
			//result = NBiometricEnginePerformTask(hBiometricClient, hBiometricTask);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error performing task");
			//}

			//// retrieve biometric task's status
			//result = NBiometricTaskGetStatus(hBiometricTask, &biometricStatus);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error getting task status");
			//}

			//if (biometricStatus != nbsOk)
			//{
			//	// retrieve biometric status
			//	result = NEnumToStringP(N_TYPE_OF(NBiometricStatus), biometricStatus, NULL, &hBiometricStatus);
			//	if (NFailed(result))
			//	{
			//		throw std::runtime_error("Biometrics: Error getting task status");
			//	}

			//	result = NStringGetBuffer(hBiometricStatus, NULL, &szBiometricStatus);
			//	if (NFailed(result))
			//	{
			//		throw std::runtime_error(szBiometricStatus);
			//	}

			//	// retrieve the error message
			//	{
			//		HNError hError = NULL;
			//		result = NBiometricTaskGetError(hBiometricTask, &hError);
			//		if (NFailed(result))
			//		{
			//			throw std::runtime_error("Biometrics: Error getting task status");
			//		}
			//		result = N_E_FAILED;
			//		if (hError)
			//		{
			//			result = NObjectSet(NULL, (HNObject *)&hError);
			//			throw std::runtime_error("Biometrics: task error");
			//		}
			//	}
			//}

			//LARGE_INTEGER begin, end, freq;
			//QueryPerformanceCounter(&begin);


			//TimedRun([this, buffer2, packedSize2, ppMatchDetails, &score](){NMVerify(hMatcher, buffer, packedSize, buffer2, packedSize2, ppMatchDetails, &score);}, "Matching: Time elapsed");
			//result = NMVerify(hMatcher, enrolledTemplate, (NSizeType)enrolledTemplateSize, prescannedTemplate, (NSizeType)prescannedTemplateSize, ppMatchDetails, &score);
			//__try {
				//result = NBiometricEngineVerifyOffline(hBiometricClient, hProbeSubject, hGallerySubject, &biometricStatus);

			result = NBiometricEngineVerifyOffline(hBiometricClient, hGallerySubject, hProbeSubject, &biometricStatus);

			//result = NBiometricEngineIdentify(hBiometricClient, hProbeSubject, &biometricStatus);
			//} __except(EXCEPTION_EXECUTE_HANDLER) {
			//	throw std::runtime_error("Biometrics Exception");
			//}

			//QueryPerformanceCounter(&end);
			//QueryPerformanceFrequency(&freq);
			//double res = (end.QuadPart - begin.QuadPart) / (double) freq.QuadPart;
			//std::stringstream ss; 
			//ss << res << " sec  ======================================================\n";
			//OutputDebugString(ss.str().c_str());

			if (NFailed(result))
			{
				//printStatusStatement("Error matching records:", result);
				throw std::runtime_error("Biometrics: Error matching records");
			} 
			
			if (biometricStatus == nbsOk) {
				// retrieve matching results array for hProbeSubject
				result = NSubjectGetMatchingResult(hProbeSubject, 0, &hMatchingResult);
				//result = NSubjectGetMatchingResults(hProbeSubject, &hMatchingResults, &resultsCount);
				if (NFailed(result))
				{
					//printStatusStatement("Error retrieve matching results from emrolled template:", result);
					throw std::runtime_error("Biometrics: Error retrieving matching results from probe template");
				}

				//if (resultsCount > 0) {

				//	result = NMatchingResultGetId(hMatchingResults[0], &hMatchId);
				//	if (NFailed(result))
				//	{
				//		throw std::runtime_error("Biometrics: Error getting a matched id");
				//	}

					// retrieve matching score from matching results
					//result = NMatchingResultGetScore(hMatchingResults[0], &matchScore);
					result = NMatchingResultGetScore(hMatchingResult, &matchScore);
					if (NFailed(result))
					{
						throw std::runtime_error("Biometrics: Error retrieving matching score from matching results");
					}

					//// free unneeded hMatchId
					//result = NStringSet(NULL, &hMatchId);
					//if (NFailed(result))
					//{
					//	throw std::runtime_error("Biometrics: Error deleting a matched id");
					//}
				//}

				result = NObjectSet(NULL, (HNObject *)&hMatchingResult);
				//result = NObjectUnrefArray((HNObject *)hMatchingResults, resultsCount);
				if (NFailed(result))
				{
					//printStatusStatement("Error clearing resources");
					throw std::runtime_error("Biometrics: Error deleting matching result");
				}
			}
			//else
			//{
			//	// retrieve biometric status
			//	result = NEnumToStringP(N_TYPE_OF(NBiometricStatus), biometricStatus, NULL, &hBiometricStatus);
			//	if (NFailed(result))
			//	{
			//		throw std::runtime_error("Biometrics (Matching): identification failed");
			//	}

			//	result = NStringGetBuffer(hBiometricStatus, NULL, &szBiometricStatus);
			//	if (NFailed(result))
			//	{
			//		throw std::runtime_error(szBiometricStatus);
			//	}

			//	// retrieve the error message
			//	{
			//		HNError hError = NULL;
			//		result = NBiometricTaskGetError(hBiometricTask, &hError);
			//		if (NFailed(result))
			//		{
			//			throw std::runtime_error("Biometrics (Matching): Error getting task status");
			//		}
			//		result = N_E_FAILED;
			//		if (hError)
			//		{
			//			result = NObjectSet(NULL, (HNObject *)&hError);
			//			throw std::runtime_error("Biometrics (Matching): task error");
			//		}
			//	}
			//}

			// free unneeded hGallerySubject
			result = NObjectSet(NULL, (HNObject *)&hGallerySubject);
			if (NFailed(result))
			{
				throw std::runtime_error("Biometrics: Error deleting gallery subject");
			}

			//// free unneeded hSubjectId
			//result = NStringSet(NULL, &hSubjectId);
			//if (NFailed(result))
			//{
			//	throw std::runtime_error("Biometrics: Error deleting gallery subject id");
			//}

			//result = NObjectSet(NULL, (HNObject *)&hBiometricTask);
			//if (NFailed(result))
			//{
			//	//printStatusStatement("Error clearing resources");
			//	throw std::runtime_error("Biometrics: Error clearing resources");
			//}

			//result = NObjectSet(NULL, (HNObject *)&hBiometricClient);
			//if (NFailed(result))
			//{
			//	//printStatusStatement("Error clearing resources");
			//	throw std::runtime_error("Biometrics: deleting BiometricClient");
			//}

			if (matchScore > 0 && biometricStatus == nbsOk)
			{
				return true;
				//matchingStatus = true;
				//printStatusStatement("templates match");
				//sprintf_s (statusStatement, "templates match! score: %d", score);
			}
			else
			{
				return false;
				//printStatusStatement("templates do not match");
				//sprintf_s (statusStatement, "templates do not match. score: %d", score);
			}
		}
	}
}