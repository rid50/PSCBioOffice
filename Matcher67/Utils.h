#ifndef SAMPLE_UTILS_H_INCLUDED
#define SAMPLE_UTILS_H_INCLUDED

#include <NTypes.h>

#ifdef N_CPP
extern "C"
{
#endif

typedef struct CfgEntry_
{
	NChar *Name;
	NChar *Value;
} CfgEntry;

#ifndef N_WINDOWS
void Sleep(unsigned long milliseconds);

int _ftprintf(FILE *stream, const NChar *format, ...);
int _tprintf(const NChar *format, ...);
#ifdef N_UNICODE
	FILE * wfopen(const wchar_t * filename, const wchar_t * access);
#endif
#endif

void onStart(const NChar *title, const NChar *description, const NChar *version, const NChar *copyright);
void onExit();

NResult obtainLicense(const NChar** licenseList, NInt count);
NResult releaseLicense(const NChar** licenseList, NInt count);

void printError(NResult result);
void printErrorMsg(NChar *errorMessage, NResult result);

NByte* readAllBytes(const NChar *fileName, NSizeType *readSize);
NBool writeAllBytes(const NChar *fileName, const NByte *buffer, NSizeType bufferSize);

NChar* CfgGetValue(CfgEntry * entries, const NChar * valueName);
CfgEntry* loadConfigurations(NChar *path);

NChar * formatTime(NLong dateTime);

#ifdef N_CPP
}
#endif

#endif // SAMPLE_UTILS_H_INCLUDED
