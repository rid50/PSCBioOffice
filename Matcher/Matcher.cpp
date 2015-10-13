//#include "stdafx.h"

#include "Utils.h"
#include "Matcher.h"

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

//	result = NfeCreate(&hExtractor);

//	if (result != N_OK) {
//		printStatusStatement("Error creating the Extractor");
//		return;
//	}

//	result = NMCreate(&hMatcher);

//	if (result != N_OK) {
//		printStatusStatement("Error creating the Matcher");
//		return;
//	}

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

		bool MatcherFacade::match(void *buffer2, int size) {
			return static_cast<Nomad::Bio::Matcher*>(matcherPtr)->match(buffer2, size);
		}

		void match(void *buffer2, int size);
		Matcher::~Matcher() {
			//if (hExtractor != NULL)
			//	NObjectFree(hExtractor);
			if (hMatcher != NULL)
				NObjectFree(hMatcher);

			//if (imageMap[1] != NULL)
			//	NObjectFree(imageMap[1]);
			//if (imageMap[2] != NULL)
			//	NObjectFree(imageMap[2]);

			//imageMap.clear();
			//int i = 5;
			//printStatusStatement("Kuku2");
		};

		NSizeType	Matcher::packedSize = 0;
		void		*Matcher::buffer = 0;

		Matcher::Matcher() {
			hImageFile = NULL;
			hImageFile2 = NULL;
			hImage = NULL;
			hImage2 = NULL;
			//buffer = NULL;
			buffer2 = NULL;
			hRecord = NULL;
			hRecord2 = NULL;

			hExtractor = 0;
			hMatcher = 0;

			//int i = 5;
			//printStatusStatement("Kuku");

			NResult result;
			const NChar * LicensesMain[] = { L"FingersExtractor", L"FingersMatcher" };
			const NChar * LicensesBSS[] = { L"FingersBSS" };

			result = obtainLicense(LicensesMain, sizeof(LicensesMain)/sizeof(LicensesMain[0]));
			if (NFailed(result))
			{
				if (result == N_E_FAILED)
				{
					printStatusStatement("Failed to obtain licenses for extractor and/or matcher");
				}
				else
				{
					printStatusStatement("Licensing manager is not running");
				}
			}
			else
			{
				printStatusStatement("Licenses for extractor and/or matcher obtained");
			}

			result = obtainLicense(LicensesBSS, sizeof(LicensesBSS)/sizeof(LicensesBSS[0]));
			if (NSucceeded(result))
			{
				printStatusStatement("Licenses for BSS obtained");
			}

			//result = NfeCreate(&hExtractor);

			//if (result != N_OK) {
			//	printStatusStatement("Error creating the Extractor");
			//	return;
			//}

			result = NMCreate(&hMatcher);

			if (result != N_OK) {
				printStatusStatement("Error creating the Matcher");
				return;
			}
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
			buffer = record;
			packedSize = size;

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

		bool Matcher::match(void *buffer2, int size) {

			NInt score;
			NMMatchDetails **ppMatchDetails = NULL;
			NSizeType packedSize2 = size;

			//TimedRun([this, buffer2, packedSize2, ppMatchDetails, &score](){NMVerify(hMatcher, buffer, packedSize, buffer2, packedSize2, ppMatchDetails, &score);}, "Matching: Time elapsed");
			result = NMVerify(hMatcher, buffer, packedSize, buffer2, packedSize2, ppMatchDetails, &score);
			result = N_OK;

			if (result != N_OK) {
				printStatusStatement("Error matching the records");
				clean();
				return false;
			}

			if (score > 0)
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

		void Matcher::clean() {
			if (buffer != NULL)
				free(buffer);
			if (buffer2 != NULL)
				free(buffer2);
			if (hRecord != NULL)
				NObjectFree(hRecord);
			if (hRecord2 != NULL)
				NObjectFree(hRecord2);
			//		if (hImage != NULL)
			//			NObjectFree(hImage);
			//		if (hImage2 != NULL)
			//			NObjectFree(hImage2);
			if (hImageFile != NULL) {
				NImageFileClose(hImageFile);
				NObjectFree(hImageFile);
				hImageFile = NULL;
			}
			if (hImageFile2 != NULL) {
				NImageFileClose(hImageFile2);
				NObjectFree(hImageFile2);
				hImageFile2 = NULL;
			}

		}

		void Matcher::checkExtractionStatus(const NfeExtractionStatus &extractionStatus) {
			if (extractionStatus == nfeesTemplateCreated)
			{
				printStatusStatement("extracted");
			}
			else
			{
				switch (extractionStatus)
				{
				case nfeesTooFewMinutiae:
					printStatusStatement("failed to extract (too few minutiae)");
					break;
				case nfeesQualityCheckFailed:
					printStatusStatement("failed to extract (quality check failed)");
					break;
				case nfeesMatchingFailed:
					printStatusStatement("failed to extract (matching failed)");
					break;
				default:
					printStatusStatement("failed to extract");
					break;
				}
			}
		}
	}
}