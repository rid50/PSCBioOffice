//#include "stdafx.h"

//#include "Utils.h"
#include "Matcher.h"

//using namespace Neurotec;
//using namespace Neurotec::Licensing;

//using namespace std;

//BOOL APIENTRY DllMain(HANDLE hModule, 
//                    DWORD  ul_reason_for_call, 
//                    LPVOID lpReserved)
//{
//	switch( ul_reason_for_call ) {
//		case DLL_THREAD_ATTACH:
//			getLicenses();
//		case DLL_THREAD_DETACH:
//			break;
//		case DLL_PROCESS_ATTACH:
//		case DLL_PROCESS_DETACH:
//			break;
//	}
//	return TRUE;
//}

//void getLicenses() {
//	NResult result;
//	const NChar * LicensesMain[] = { L"FingersExtractor", L"FingersMatcher" };
//	const NChar * LicensesBSS[] = { L"FingersBSS" };
//
//	result = obtainLicense(LicensesMain, sizeof(LicensesMain)/sizeof(LicensesMain[0]));
//	if (NFailed(result))
//	{
//		if (result == N_E_FAILED)
//		{
//			printStatusStatement("Failed to obtain licenses for extractor and/or matcher");
//		}
//		else
//		{
//			printStatusStatement("Licensing manager is not running");
//		}
//	}
//	else
//	{
//		printStatusStatement("Licenses for extractor and/or matcher obtained");
//	}
//
//	result = obtainLicense(LicensesBSS, sizeof(LicensesBSS)/sizeof(LicensesBSS[0]));
//	if (NSucceeded(result))
//	{
//		printStatusStatement("Licenses for BSS obtained");
//	}
//
//	result = NfeCreate(&hExtractor);
//
//	if (result != N_OK) {
//		printStatusStatement("Error creating the Extractor");
//		return;
//	}
//
//	result = NMCreate(&hMatcher);
//
//	if (result != N_OK) {
//		printStatusStatement("Error creating the Matcher");
//		return;
//	}
//
//}

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
			//std::cout << "Kuku3" << std::endl;
		};

		MatcherFacade::MatcherFacade() {
			//Nomad::Bio::Matcher* matcher = new Nomad::Bio::Matcher();
			matcherPtr = new Nomad::Bio::Matcher();
		};

		void MatcherFacade::enroll(unsigned char *record, unsigned __int32 size) {
			//static_cast<Nomad::Bio::Matcher*>(matcherPtr)->enroll(record, size);
			Matcher::enroll(record, size);
		}

		bool MatcherFacade::match(void *prescannedTemplate, unsigned __int32 prescannedTemplateSize) {
			return static_cast<Nomad::Bio::Matcher*>(matcherPtr)->match(prescannedTemplate, prescannedTemplateSize);
		}

		////void match(void *buffer2, int size);
		//License::License() {
		//	const NChar * LicensesMain[] = { L"Biometrics.FingerMatching" };
		//	//const NChar * LicensesBSS[] = { L"FingersBSS" };

		//	result = obtainLicense(LicensesMain, sizeof(LicensesMain)/sizeof(LicensesMain[0]));
		//	if (NFailed(result))
		//	{
		//		if (result == N_E_FAILED)
		//		{
		//			printStatusStatement("Failed to obtain licenses for extractor and/or matcher");
		//		}
		//		else
		//		{
		//			printStatusStatement("Licensing manager is not running");
		//		}
		//	}
		//	else
		//	{
		//		printStatusStatement("Licenses for extractor and/or matcher obtained");
		//	}

		//	result = obtainLicense(LicensesBSS, sizeof(LicensesBSS)/sizeof(LicensesBSS[0]));
		//	if (NSucceeded(result))
		//	{
		//		printStatusStatement("Licenses for BSS obtained");
		//	}
		//}


		NSizeType	Matcher::enrolledTemplateSize = 0;
		void		*Matcher::enrolledTemplate = 0;

		const NChar * components = N_T("Biometrics.FingerExtraction,Biometrics.FingerMatching");

		Matcher::~Matcher() {
			NObjectSet(NULL, (HNObject *)&hProbeSubject);
			NObjectSet(NULL, (HNObject *)&hGallerySubject);
			NObjectSet(NULL, (HNObject *)&hMatchingResults);
			NLicenseReleaseComponents(components);


			//if (hExtractor != NULL)
			//	NObjectFree(hExtractor);
			//if (hMatcher != NULL)
			//	NObjectFree(hMatcher);

			//if (imageMap[1] != NULL)
			//	NObjectFree(imageMap[1]);
			//if (imageMap[2] != NULL)
			//	NObjectFree(imageMap[2]);

			//imageMap.clear();
			//int i = 5;
			//printStatusStatement("Kuku2");
		};

		Matcher::Matcher() {
			//hImageFile = NULL;
			//hImageFile2 = NULL;
			//hImage = NULL;
			//hImage2 = NULL;
			////buffer = NULL;
			//buffer2 = NULL;
			//hRecord = NULL;
			//hRecord2 = NULL;

			//hExtractor = 0;
			//hMatcher = 0;

			//int i = 5;
			//printStatusStatement("Kuku");

			//NResult result;
			//const NChar * LicensesMain[] = { L"FingersExtractor", L"FingersMatcher" };
			//const NChar * LicensesBSS[] = { L"FingersBSS" };

			//result = obtainLicense(LicensesMain, sizeof(LicensesMain)/sizeof(LicensesMain[0]));
			//if (NFailed(result))
			//{
			//	if (result == N_E_FAILED)
			//	{
			//		printStatusStatement("Failed to obtain licenses for extractor and/or matcher");
			//	}
			//	else
			//	{
			//		printStatusStatement("Licensing manager is not running");
			//	}
			//}
			//else
			//{
			//	printStatusStatement("Licenses for extractor and/or matcher obtained");
			//}

			//result = obtainLicense(LicensesBSS, sizeof(LicensesBSS)/sizeof(LicensesBSS[0]));
			//if (NSucceeded(result))
			//{
			//	printStatusStatement("Licenses for BSS obtained");
			//}

			//result = NfeCreate(&hExtractor);

			//if (result != N_OK) {
			//	printStatusStatement("Error creating the Extractor");
			//	return;
			//}

			//try {
			//	NLicense::ObtainComponents("/local", "5000", "Biometrics.FingerMatching");
			//}
			//catch(NError& ex)
			//{
			//	std::stringstream stmt;
			//	stmt << "Failed to obtain licenses for components:" << (std::string)ex.ToString();

			//	printStatusStatement(stmt.str().c_str());
			//}
			
			NBool available = NFalse;

			result = NLicenseObtainComponents(N_T("/local"), N_T("5000"), components, &available);
			if (NFailed(result))
			{
				printStatusStatement("Failed to obtain licenses for components:", result);
				throw std::runtime_error("Biometrics: Failed to obtain licenses");
			}

			if (!available)
			{
				std::stringstream stmt;
				stmt << "Biometrics: Licenses for " << components << " are not available";
				printStatusStatement(stmt.str().c_str());
				throw std::runtime_error(stmt.str().c_str());
			}
			//static License license;

			//result = NMCreate(&hMatcher);

			//if (result != N_OK) {
			//	printStatusStatement("Error creating the Matcher");
			//	return;
			//}
		}


		//bool Matcher::getMatchingStatus() {
		//	return matchingStatus;
		//}

		//bool Matcher::readImages(wchar_t * szFileName, wchar_t * szFileName2) {
		//	hImageFile = NULL;
		//	hImageFile2 = NULL;
		//	hImage = NULL;
		//	hImage2 = NULL;

		//	matchingStatus = false;
		//	//wchar_t * szFileName;
		//	//szFileName = L"C:\\roman\\psc\\wsq\\llittle.wsq";
		//	//szFileName = L"C:\\roman\\psc\\wsq\\lindex.wsq";
		//	//szFileName = L"C:\\roman\\psc\\wsq\\rthumb.wsq";
		//	//szFileName2 = L"C:\\roman\\psc\\wsq\\rthumb.wsq";

		//	HNImageFormat hImageFormat = NULL;
		//	result = NImageFileCreate(szFileName, hImageFormat, &hImageFile);
		//	if (result != N_OK) {
		//		printStatusStatement("Error image file opening");
		//		clean();
		//		return false;
		//	}

		//	result = NImageFileCreate(szFileName2, hImageFormat, &hImageFile2);
		//	if (result != N_OK) {
		//		printStatusStatement("Error image file opening");
		//		clean();
		//		return false;
		//	}

		//	NImageFileReadImage(hImageFile, &hImage);
		//	if (result != N_OK) {
		//		printStatusStatement("Error image file reading");
		//		clean();
		//		return false;
		//	}

		//	NImageFileReadImage(hImageFile2, &hImage2);
		//	if (result != N_OK) {
		//		printStatusStatement("Error image2 file reading");
		//		clean();
		//		return false;
		//	}

		//	imageMap.insert ( std::make_pair( 1, hImage ) );
		//	imageMap.insert ( std::make_pair( 2, hImage2 ) );

		//	clean();
		//	return true;
		//}

		//int Matcher::extract(HNImage nImage, NfeExtractionStatus* extractionStatus, HNFRecord* hRecord) {
		//	//hRecord = NULL;
		//	//NFPosition position = nfpUnknown;
		//	//NFImpressionType impressionType = nfitNonliveScanPlain;
		//	//NfeExtractionStatus extractionStatus;
		//	//result = NfeExtract(hExtractor, hImage, position, impressionType, &extractionStatus, &hRecord);
		//	return NfeExtract(hExtractor, nImage, nfpUnknown, nfitNonliveScanPlain, extractionStatus, hRecord);
		//	//if (result != N_OK) {
		//	//	printStatusStatement("Error extracting a record");
		//	//	return -1;
		//	//} else	if (extractionStatus != nfeesTemplateCreated) {
		//	//	printStatusStatement("Record 1:");
		//	//	checkExtractionStatus(extractionStatus);
		//	//	return -1;
		//	//}

		//	//return 0;
		//}

		void Matcher::enroll(unsigned char *record, unsigned __int32 size) {
			enrolledTemplate = record;
			enrolledTemplateSize = size;

			//wchar_t * szFileName = L"C:\\roman\\psc\\wsq\\lindex.wsq";
			//std::wcout << szFileName  << std::endl;
			//HNImageFormat hImageFormat = NULL;
			//result = NImageFileCreate(szFileName, hImageFormat, &hImageFile);
			//if (result != N_OK) {
			//	printStatusStatement("Error image file opening");
			//	clean();
			//	return;
			//}

			//NImageFileReadImage(hImageFile, &hImage);
			//if (result != N_OK) {
			//	printStatusStatement("Error image file reading");
			//	clean();
			//	return;
			//}

			//NfeExtractionStatus extractionStatus;
			//result = extract(hImage, &extractionStatus, &hRecord);
			//if (result != N_OK) {
			//	printStatusStatement("Error extracting a record");
			//	clean();
			//	return;
			//} else if (extractionStatus != nfeesTemplateCreated) {
			//	printStatusStatement("Record 1:");
			//	checkExtractionStatus(extractionStatus);
			//	clean();
			//	return;
			//}
			//else
			//{
			//	NSizeType bufferSize;
			//	NUInt flags = 0;
			//	NFRecordGetSize(hRecord, flags, &bufferSize);
			//	buffer = malloc(bufferSize);
			//	//NSizeType packedSize;
			//	result = NFRecordSaveToMemory(hRecord, buffer, bufferSize, flags, &packedSize);
			//	if (result != N_OK) {
			//		printStatusStatement("Error saving a record in memory buffer");
			//		clean();
			//		return;
			//	}
			//}
		}

		bool Matcher::match(void *prescannedTemplate, unsigned __int32 prescannedTemplateSize) {
			NInt matchScore = 0;
			hBiometricClient = NULL;
			hMatchingResults = NULL;
			NBiometricStatus biometricStatus = nbsNone;

			hProbeSubject = NULL;
			hGallerySubject = NULL;

			//NMMatchDetails **ppMatchDetails = NULL;
			//NSizeType packedSize2 = prescannedTemplateSize;


			// create biometric client
			result = NBiometricClientCreate(&hBiometricClient);
			if (NFailed(result))
			{
				printStatusStatement("Biometric client creation failed:", result);
				throw std::runtime_error("Biometric client creation failed");
			}

			NInt matchingThreshold = 48;
			NMatchingSpeed matchingSpeed = nmsLow;
			//NMatchingSpeed matchingSpeed = nmsHigh;

			// set matching threshold
			result = NObjectSetPropertyP(hBiometricClient, N_T("Matching.Threshold"), N_TYPE_OF(NInt32), naNone, &matchingThreshold, sizeof(matchingThreshold), 1, NTrue);
			if (NFailed(result))
			{
				printStatusStatement("Error setting matching threshold:", result);
				throw std::runtime_error("Biometrics: Error setting matching threshold");
			}

			// set matching speed
			result = NObjectSetPropertyP(hBiometricClient, N_T("Fingers.MatchingSpeed"), N_TYPE_OF(NMatchingSpeed), naNone, &matchingSpeed, sizeof(matchingSpeed), 1, NTrue);
			if (NFailed(result))
			{
				printStatusStatement("Error setting matching speed:", result);
				throw std::runtime_error("Biometrics: Error setting matching speed");
			}

			NSizeType pSize = 0;
			result = NSubjectCreateFromMemory(enrolledTemplate, (NSizeType)enrolledTemplateSize, 0, &pSize, &hProbeSubject);
			if (NFailed(result))
			{
				printStatusStatement("Error creating a subject for enrolled template:", result);
				throw std::runtime_error("Biometrics: Error creating a subject for enrolled template");
			}

			NSizeType pSize2 = 0;
			result = NSubjectCreateFromMemory(prescannedTemplate, (NSizeType)prescannedTemplateSize, 0, &pSize2, &hGallerySubject);
			if (NFailed(result))
			{
				printStatusStatement("Error creating a subject for prescanned template:", result);
				throw std::runtime_error("Biometrics: Error creating a subject for prescanned template");
			}


			//TimedRun([this, buffer2, packedSize2, ppMatchDetails, &score](){NMVerify(hMatcher, buffer, packedSize, buffer2, packedSize2, ppMatchDetails, &score);}, "Matching: Time elapsed");
			//result = NMVerify(hMatcher, enrolledTemplate, (NSizeType)enrolledTemplateSize, prescannedTemplate, (NSizeType)prescannedTemplateSize, ppMatchDetails, &score);
			result = NBiometricEngineVerifyOffline(hBiometricClient, hProbeSubject, hGallerySubject, &biometricStatus);
			//if (NFailed(result) || biometricStatus != nbsOk)
			if (NFailed(result))
			{
				printStatusStatement("Error matching records:", result);
				throw std::runtime_error("Biometrics: Error matching records");
			} else if (biometricStatus == nbsOk) {
				// retrieve matching results from hProbeSubject
				result = NSubjectGetMatchingResult(hProbeSubject, 0, &hMatchingResults);
				if (NFailed(result))
				{
					printStatusStatement("Error retrieve matching results from emrolled template:", result);
					throw std::runtime_error("Biometrics: Error retrieve matching results from emrolled template");
				}

				// retrieve matching score from matching results
				result = NMatchingResultGetScore(hMatchingResults, &matchScore);
				if (NFailed(result))
				{
					printStatusStatement("Error retrieve matching score from matching results:", result);
					throw std::runtime_error("Biometrics: Error retrieve matching score from matching results");
				}

				//printf(N_T("\nimage scored %d, verification.. "), matchScore);

				//if (matchScore > 0)
				//	printf(N_T("succeeded\n"));
				//else
				//	printf(N_T("failed\n"));
			} else
				return false;

			//if (result != N_OK) {
			//	printStatusStatement("Error matching the records");
			//	//clean();
			//	return false;
			//}

			if (matchScore > 0)
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

		//void Matcher::run() {
		//	buffer = NULL;
		//	buffer2 = NULL;
		//	hRecord = NULL;
		//	hRecord2 = NULL;

		//	//hImage = NULL;
		//	//		hImage = imageMap.at(1);
		//	//		hImage2 = imageMap[2];
		//	//HNGrayscaleImage hGrayImage;
		//	NFPosition position = nfpUnknown;
		//	NFImpressionType impressionType = nfitNonliveScanPlain;
		//	NfeExtractionStatus extractionStatus;
		//	//result = NfeExtract(hExtractor, hImage, position, impressionType, &extractionStatus, &hRecord);
		//	//result = NfeExtract(hExtractor, imageMap.at(1), position, impressionType, &extractionStatus, &hRecord);

		//	result = extract(imageMap.at(1), &extractionStatus, &hRecord);

		//	if (result != N_OK) {
		//		printStatusStatement("Error extracting a record");
		//		clean();
		//		return;
		//	}

		//	if (extractionStatus != nfeesTemplateCreated) {
		//		printStatusStatement("Record 1:");
		//		checkExtractionStatus(extractionStatus);
		//	}
		//	else
		//	{
		//		//result = NfeExtract(hExtractor, hImage2, position, impressionType, &extractionStatus, &hRecord2);
		//		//result = NfeExtract(hExtractor, imageMap.at(2), position, impressionType, &extractionStatus, &hRecord2);
		//		//TimedRun([this, position, impressionType, &extractionStatus](){NfeExtract(hExtractor, imageMap.at(2), position, impressionType, &extractionStatus, &hRecord2);}, "Extracting: Time elapsed");

		//		TimedRun([this, position, impressionType, &extractionStatus](){extract(imageMap.at(2), &extractionStatus, &hRecord2);}, "Extracting: Time elapsed");
		//		result = N_OK;

		//		if (result != N_OK) {
		//			printStatusStatement("Error extracting a record2");
		//			clean();
		//			return;
		//		}

		//		if (extractionStatus != nfeesTemplateCreated) {
		//			printStatusStatement("Record 2:");
		//			checkExtractionStatus(extractionStatus);
		//		}
		//		else
		//		{
		//			NSizeType bufferSize, bufferSize2;
		//			NUInt flags = 0;
		//			NFRecordGetSize(hRecord, flags, &bufferSize);
		//			buffer = malloc(bufferSize);
		//			NSizeType packedSize, packedSize2;
		//			result = NFRecordSaveToMemory(hRecord, buffer, bufferSize, flags, &packedSize);
		//			if (result != N_OK) {
		//				printStatusStatement("Error saving a record in memory buffer");
		//				clean();
		//				return;
		//			}

		//			NFRecordGetSize(hRecord2, flags, &bufferSize2);
		//			buffer2 = malloc(bufferSize2);
		//			result = NFRecordSaveToMemory(hRecord2, buffer2, bufferSize2, flags, &packedSize2);
		//			if (result != N_OK) {
		//				printStatusStatement("Error saving a record2 in memory buffer");
		//				clean();
		//				return;
		//			}

		//			NInt score;
		//			NMMatchDetails **ppMatchDetails = NULL;

		//			//result = NMVerify(hMatcher, buffer, packedSize, buffer2, packedSize2, ppMatchDetails, &score);
		//			TimedRun([this, packedSize, packedSize2, ppMatchDetails, &score](){NMVerify(hMatcher, buffer, packedSize, buffer2, packedSize2, ppMatchDetails, &score);}, "Matching: Time elapsed");
		//			result = N_OK;

		//			if (result != N_OK) {
		//				printStatusStatement("Error matching the records");
		//				clean();
		//				return;
		//			}

		//			if (score > 0)
		//			{
		//				matchingStatus = true;
		//				printStatusStatement("templates match");
		//				//sprintf_s (statusStatement, "templates match! score: %d", score);
		//			}
		//			else
		//			{
		//				printStatusStatement("templates do not match");
		//				//sprintf_s (statusStatement, "templates do not match. score: %d", score);
		//			}

		//			//sprintf_s (sBuff, "Ok");
		//			//szMatchingStatus = sBuff;
		//			//cout << sBuff << std::endl;

		//		}
		//	}

		//	//dynamic_cast<wxNGrayscaleImage *>(grayImage.get())

		//	//ProcessImage(fileName, image);
		//	clean();
		//}

		//void Matcher::clean() {
		//	if (buffer != NULL)
		//		free(buffer);
		//	if (buffer2 != NULL)
		//		free(buffer2);
		//	if (hRecord != NULL)
		//		NObjectFree(hRecord);
		//	if (hRecord2 != NULL)
		//		NObjectFree(hRecord2);
		//	//		if (hImage != NULL)
		//	//			NObjectFree(hImage);
		//	//		if (hImage2 != NULL)
		//	//			NObjectFree(hImage2);
		//	if (hImageFile != NULL) {
		//		NImageFileClose(hImageFile);
		//		NObjectFree(hImageFile);
		//		hImageFile = NULL;
		//	}
		//	if (hImageFile2 != NULL) {
		//		NImageFileClose(hImageFile2);
		//		NObjectFree(hImageFile2);
		//		hImageFile2 = NULL;
		//	}

		//}

		//void Matcher::checkExtractionStatus(const NfeExtractionStatus &extractionStatus) {
		//	if (extractionStatus == nfeesTemplateCreated)
		//	{
		//		printStatusStatement("extracted");
		//	}
		//	else
		//	{
		//		switch (extractionStatus)
		//		{
		//		case nfeesTooFewMinutiae:
		//			printStatusStatement("failed to extract (too few minutiae)");
		//			break;
		//		case nfeesQualityCheckFailed:
		//			printStatusStatement("failed to extract (quality check failed)");
		//			break;
		//		case nfeesMatchingFailed:
		//			printStatusStatement("failed to extract (matching failed)");
		//			break;
		//		default:
		//			printStatusStatement("failed to extract");
		//			break;
		//		}
		//	}
		//}
	}
}