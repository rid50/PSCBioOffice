#include <NCore.h>

// system headers
#ifndef N_WINDOWS
#  define _GNU_SOURCE
#else
#ifndef _CRT_SECURE_NO_WARNINGS
#define _CRT_SECURE_NO_WARNINGS
#endif // !defined(_CRT_SECURE_NO_WARNINGS)
#ifndef _CRT_NON_CONFORMING_SWPRINTFS
#define _CRT_NON_CONFORMING_SWPRINTFS
#endif // !defined(_CRT_NON_CONFORMING_SWPRINTFS)
#endif

#include <stdio.h>
#include <stdlib.h>
#include <time.h>

#ifdef N_WINDOWS
#include <tchar.h>
#include <Windows.h>
#else
#include <sys/param.h>
#include <limits.h>
#include <dlfcn.h>
#include <libgen.h>
#include <dirent.h>
#include <errno.h>
#include <ctype.h>
#include <string.h>
#ifdef _UNICODE
#include <wchar.h>
#include <wctype.h>
#define _tcscmp wcscmp
#define _tcscpy wcscpy
#define _tcscat wcscat
#define _tcslen wcslen
#define _tcsdup wcsdup
#define _fgetts fgetws
#define _tfopen wfopen
#define _sntprintf swprintf
#define _tcsftime wcsftime
#else
#define _tcscmp strcmp
#define _tcscpy strcpy
#define _tcscat strcat
#define _tcslen strlen
#define _tcsdup strdup
#define _fgetts fgets
#define _tfopen fopen
#define _sntprintf snprintf
#define _tcsftime strftime
#endif
#endif

#ifndef N_PRODUCT_HAS_NO_LICENSES
#include <NLicensing.h>
#endif

#include "Utils.h"

#if defined(N_MAC) && defined(N_UNICODE)
wchar_t * wcsdup(const wchar_t * str);
#endif

static CfgEntry* CfgLoad();

static CfgEntry * _entries = NULL;

void onStart(const NChar *title, const NChar *description, const NChar *version, const NChar *copyright)
{
	_tprintf(N_T("%s tutorial\n"), title);
	_tprintf(N_T("description: %s\n"), description);
	_tprintf(N_T("version: %s\n"), version);
	_tprintf(N_T("copyright: %s\n\n"), copyright);

	NCoreOnStart();
}

void onExit()
{
	NCoreOnExit();
}

NResult obtainLicense(const NChar ** licenseList, NInt count)
{
#ifndef N_PRODUCT_HAS_NO_LICENSES
	NResult result;
	NBool available;
	NInt i, j;

	NChar * address, * port, * license;

	if (_entries == NULL)
	{
		_entries = CfgLoad();
		if (_entries == NULL)
		{
			_ftprintf(stderr, N_T("failed to load license configuration file\n"));
			return N_E_FAILED;
		}
	}

	// Map licenses back
	for (i = 0; i < count; i++)
	{
		license = CfgGetValue(_entries, licenseList[i]);
		if (license == NULL)
		{
			licenseList[i] = N_T("");
		}
		else
		{
			licenseList[i] = license;
		}
	}

	// Remove duplicates from the array
	for (i = 0; i < count - 1; i++)
	{
		if (_tcscmp(licenseList[i], N_T("")) == 0)
		{
			continue;
		}

		for (j = i + 1; j < count; j++)
		{
			if (_tcscmp(licenseList[i], licenseList[j]) == 0)
			{
				licenseList[j] = N_T("");
			}
		}
	}

	address = CfgGetValue(_entries, N_T("Address"));
	port = CfgGetValue(_entries, N_T("Port"));

	for (i = 0; i < count; i++)
	{
		if (_tcscmp(licenseList[i], N_T("")) == 0)
		{
			continue;
		}

		result = NLicenseObtain(address, port, licenseList[i], &available);
		if (NFailed(result))
		{
			printErrorMsg(N_T("NLicenseObtain() failed, result = %d\n"), result);
			return result;
		}
		//_ftprintf(stderr, N_T("license for %s %s\n"), licenseList[i], available ? N_T("obtained") : N_T("not available"));
		if (!available)
		{
			// If not available
			return N_E_FAILED;
		}
	}
#else
	N_UNREFERENCED_PARAMETER(licenseList);
	N_UNREFERENCED_PARAMETER(count);
#endif // !N_PRODUCT_HAS_NO_LICENSES

	return N_OK;
}

NResult releaseLicense(const NChar** licenseList, NInt count)
{
#ifndef N_PRODUCT_HAS_NO_LICENSES
	NResult result;
	NInt i;

	for (i = 0; i < count; i++)
	{
		if (_tcscmp(licenseList[i], N_T("")) == 0)
		{
			continue;
		}

		result = NLicenseRelease(licenseList[i]);
		if (NFailed(result))
		{
			printErrorMsg(N_T("NLicenseRelease() failed, result = %d\n"), result);
			return result;
		}
		_ftprintf(stderr, N_T("license for %s released\n"), licenseList[i]);
	}
#else
	N_UNREFERENCED_PARAMETER(licenseList);
	N_UNREFERENCED_PARAMETER(count);
#endif // !N_PRODUCT_HAS_NO_LICENSES

	return N_OK;
}

static void printCallStack(HNError error)
{
	NInt i, count, length;
	NChar *message = NULL;

	count = NErrorGetCallStackLength(error);
	if (NSucceeded(count))
	{
		for (i = 0; i != count; i++)
		{
			length = NErrorGetCallStackFunctionA(error, i, NULL);
			if (NFailed(length))
			{
				return;
			}
			message = (NChar *)malloc(sizeof(NChar) * (length + 1));
			if (message == NULL)
			{
				_ftprintf(stderr, N_T("not enough memory ...\n"));
				return;
			}
			if (NFailed(NErrorGetCallStackFunction(error, i, message)))
			{
				free(message);
				return;
			}
			_ftprintf(stderr, N_T("    at %s"), message);
			free(message);
			length = NErrorGetCallStackFile(error, i, NULL);
			if (NFailed(length))
			{
				return;
			}
			if (length > 0)
			{
				message = (NChar *)malloc(sizeof(NChar) * (length + 1));
				if (NFailed(NErrorGetCallStackFile(error, i, message)))
				{
					free(message);
					return;
				}
				length = NErrorGetCallStackLine(error, i);
				if (NFailed(length))
				{
					free(message);
					return;
				}
				_ftprintf(stderr, N_T(" in %s:line %d\n"), message, length);
				free(message);
			}
			else
			{
				_ftprintf(stderr, N_T("\n"));
			}
		}
	}
}

static NChar * getErrorMessage(HNError error)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetMessage(error, NULL);
	if (NFailed(length))
	{
		return NULL;
	}
	message = (NChar *)malloc(sizeof(NChar) * (length + 1));
	if (message == NULL)
	{
		_ftprintf(stderr, N_T("not enough memory ...\n"));
		return NULL;
	}
	if (NFailed(NErrorGetMessage(error, message)))
	{
		free(message);
		return NULL;
	}

	return message;
}

static NChar * getErrorParamMessage(HNError error)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetParam(error, NULL);
	if (NFailed(length))
	{
		return NULL;
	}
	message = (NChar *)malloc(sizeof(NChar) * (length + 1));
	if (message == NULL)
	{
		_ftprintf(stderr, N_T("not enough memory ...\n"));
		return NULL;
	}
	if (NFailed(NErrorGetParam(error, message)))
	{
		free(message);
		return NULL;
	}

	return message;
}

static NChar * getErrorExternalCallStack(HNError error)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetExternalCallStack(error, NULL);
	if (NFailed(length))
	{
		return NULL;
	}
	message = (NChar *)malloc(sizeof(NChar) * (length + 1));
	if (message == NULL)
	{
		_ftprintf(stderr, N_T("not enough memory ...\n"));
		return NULL;
	}
	if (NFailed(NErrorGetExternalCallStack(error, message)))
	{
		free(message);
		return NULL;
	}

	return message;
}

static NChar * getDefaultErrorMessage(NResult result)
{
	NInt length;
	NChar *message = NULL;
	length = NErrorGetDefaultMessage(result, NULL);
	if (NFailed(length))
	{
		return NULL;
	}
	message = (NChar *)malloc(sizeof(NChar) * (length + 1));
	if (message == NULL)
	{
		_ftprintf(stderr, N_T("not enough memory ...\n"));
		return NULL;
	}
	if (NFailed(NErrorGetDefaultMessage(result, message)))
	{
		free(message);
		return NULL;
	}
	return message;
}

static void printErrorInformation(HNError error)
{
	NResult code;
	NChar * message;
	HNError innerError;
	NInt externalError = 0;
	code = NErrorGetCode(error);
	message = getErrorMessage(error);

	_ftprintf(stderr, N_T("error code: %d\n"), code);
	if (message != NULL)
	{
		_ftprintf(stderr, N_T("error description: %s\n"), message);
		free(message);
	}

	message = getErrorParamMessage(error);
	if (message != NULL)
	{
		_ftprintf(stderr, N_T("param: %s\n"), message);
		free(message);
	}

	externalError = NErrorGetExternalError(error);
	if (externalError)
	{
		_ftprintf(stderr, N_T("external error: %d\n"), externalError);
	}

	innerError = NErrorGetInnerError(error);
	if (innerError != NULL)
	{
		_ftprintf(stderr, N_T(" ---> \n"));
		printErrorInformation(error);
		_ftprintf(stderr, N_T("\n   --- End of inner exception stack trace ---\n"));
	}

	message = getErrorExternalCallStack(error);
	if (message != NULL)
	{
		_ftprintf(stderr, N_T("%s\n"), message);
		free(message);
	}
	printCallStack(error);
}

void printError(NResult result)
{
	HNError error = NULL;

	error = NErrorGetLast();
	if ((error == NULL) || (result != NErrorGetCode(error)))
	{
		NChar * message = getDefaultErrorMessage(result);
		_ftprintf(stderr, N_T("error code: %d\n"), result);
		if (message)
		{
			_ftprintf(stderr, N_T("error description: %s\n"), message);
			free(message);
		}
	}
	else
	{
		printErrorInformation(error);
	}
}

void printErrorMsg(NChar *errorMessage, NResult result)
{
	_ftprintf(stderr, errorMessage, result);
	_ftprintf(stderr, N_T("\n"));
	printError(result);
}

NByte *readAllBytes(const NChar *fileName, NSizeType *readSize)
{
	FILE *fp;
	NSizeType bufferSize;
	NSizeType bufferRead;
	NByte *buffer;

	fp = _tfopen(fileName, N_T("rb"));
	if (!fp)
	{
		return NULL;
	}
	fseek(fp, 0, SEEK_END);
	bufferSize = ftell(fp);
	fseek(fp, 0, SEEK_SET);

	buffer = malloc(bufferSize);
	bufferRead = fread(buffer, sizeof(NByte), bufferSize, fp);
	fclose(fp);

	if (bufferRead != bufferSize)
	{
		free(buffer);
		return NULL;
	}

	*readSize = bufferSize;
	return buffer;
}

NBool writeAllBytes(const NChar *fileName, const NByte *buffer, NSizeType bufferSize)
{
	FILE *fp;
	NSizeType bytesWritten;

	fp = _tfopen(fileName, N_T("wb"));
	if (!fp)
	{
		return NFalse;
	}

	bytesWritten = fwrite(buffer, sizeof(NByte), bufferSize, fp);
	fclose(fp);

	if (bytesWritten != bufferSize)
	{
		return NFalse;
	}

	return NTrue;
}

#ifdef N_WINDOWS

static NChar* getCfgPath()
{
	NChar szDrive[_MAX_DRIVE];
	NChar szDir[_MAX_DIR];
	static NChar szPath[_MAX_PATH];

	if (!GetModuleFileName(NULL, szPath, _MAX_PATH))
	{
		return NULL;
	}

	if (_tsplitpath_s(szPath, szDrive, _MAX_DRIVE, szDir, _MAX_DIR, NULL, 0, NULL, 0 ))
	{
		return NULL;
	}

	if (_tmakepath_s(szPath, _MAX_PATH, szDrive, szDir, N_T("NLicenses"),N_T("cfg")))
	{
		return NULL;
	}

	return szPath;
}

#else

static NChar* getCfgPath()
{
	NInt ret = 0;
	Dl_info libInfo;
	NAChar libPath[MAXPATHLEN];
	NAChar *libDirPath = NULL;

	static NChar cfgPath[MAXPATHLEN];
#ifdef N_MAC
	NChar tmpCfgPath[MAXPATHLEN];
	FILE *ftmp;
#endif

	ret = dladdr((void *)getCfgPath, &libInfo);
	if (ret == 0)
	{
		return NULL;
	}

	if (libInfo.dli_fname == NULL || libInfo.dli_fname[0] == '\0')
	{
		return NULL;
	}

	strcpy(libPath, libInfo.dli_fname);

	libDirPath = dirname(libPath);

#ifdef N_UNICODE
	mbstowcs(cfgPath, libDirPath, MAXPATHLEN);
#else
	strcpy(cfgPath, libDirPath);
#endif

#ifdef N_MAC
	_tcscpy(tmpCfgPath, cfgPath);
	_tcscat(tmpCfgPath, N_T("/../Resources/NLicenses.cfg"));
	ftmp = _tfopen(tmpCfgPath, N_T("r"));
	if (ftmp)
	{
		fclose(ftmp);
		_tcscpy(cfgPath, tmpCfgPath);
		return cfgPath;
	}
#endif
	_tcscat(cfgPath, N_T("/NLicenses.cfg"));
	return cfgPath;
}
#endif

static NChar* stripWhitespaces(const NChar * str)
{
	const NChar *end;
	NSizeType out_size;
	NChar * out_str = NULL;
	NSizeType len = _tcslen(str);

	// Trim leading space
	while(isspace(*str)) str++;
	// Trim trailing space
	end = str + _tcslen(str) - 1;
	while(end > str && (isspace(*end) || *end == N_T('\r') || *end == N_T('\n'))) end--;
	end++;

	out_size = (NSizeType)(end - str) < len ? (end - str) : len;

	NCAlloc(sizeof(NChar) * (out_size + 100), (void **)&out_str);

	_tcscpy(out_str, str);
	out_str[out_size] = N_T('\0');

	return out_str;
}

static void CfgParseLine(const NChar* line, CfgEntry * pEntry)
{
	int len = 0;
	NChar name[512] = {N_T('\0'), 0};
	NChar value[512] = {N_T('\0'), 0};
	NChar * name_stripped = NULL;
	NChar * value_stripped = NULL;

	_tcscpy(name, line);

	while (*line)
	{

		if ((N_T('=') == *line))
		{
			name[len] = 0;
			name_stripped = stripWhitespaces(name);

			if (*line)
			{
				_tcscpy(value, line + 1);
				value_stripped = stripWhitespaces(value);
			}

			pEntry->Name = name_stripped;
			pEntry->Value = value_stripped;

			return;
		}
		else
		{
			line++;
			len++;
		}
	}
}

static CfgEntry* CfgLoad()
{
	CfgEntry * result, *current = NULL;
	FILE *file = NULL;
	NChar line[1024 + 2];
	NChar *path = getCfgPath();
	int lines = 0;

	if (path == NULL)
		return NULL;
	file = _tfopen(path, N_T("r"));
	if (file == NULL)
		return NULL;

	// count lines
	while (_fgetts(line, (sizeof(line) / sizeof(line[0]) - 1), file) != NULL)
	{
		lines++;
	}
	lines ++;

	result = (CfgEntry *)calloc(lines, sizeof(CfgEntry));
	current = result;

	fseek(file, 0, SEEK_SET);

	while (_fgetts(line, sizeof(line) / sizeof(line[0]), file) != NULL)
	{
		CfgParseLine(line, current);
		if (current->Name)
		{
			current ++;
		}
	}

	fclose(file);

	return result;
}

NChar* CfgGetValue(CfgEntry * entries, const NChar * valueName)
{
	CfgEntry * currentElement;

	for (currentElement = entries; currentElement->Name != NULL; currentElement++)
	{
		if (_tcscmp(currentElement->Name, valueName) == 0)
		{
			return currentElement->Value;
		}
	}

	return NULL;
}

CfgEntry* loadConfigurations(NChar *path)
{
	CfgEntry * result, *current = NULL;
	FILE *file = NULL;
	NChar line[1024 + 2];
	int lines = 0;

	if (path == NULL)
		return NULL;
	file = _tfopen(path, N_T("r"));
	if (file == NULL)
		return NULL;

	// count lines
	while (_fgetts(line, (sizeof(line) / sizeof(line[0]) - 1), file) != NULL)
	{
		lines++;
	}
	lines ++;

	result = (CfgEntry *)calloc(lines, sizeof(CfgEntry));
	current = result;

	fseek(file, 0, SEEK_SET);

	while (_fgetts(line, sizeof(line) / sizeof(line[0]), file) != NULL)
	{
		CfgParseLine(line, current);
		if (current->Name)
		{
			current ++;
		}
	}

	fclose(file);

	return result;
}

NResult decodeTime(NLong dateTime, time_t *time, NInt *miliseconds)
{
	NLong UnixEpoch = 621355968000000000LL;
	NLong TicksPerSecond = 10000000LL;
	NLong TicksPerMillisecond = 10000LL;
	NLong value1, value2;

	dateTime -= UnixEpoch;
	value1 = dateTime / TicksPerSecond;
	value2 = (dateTime - value1 * TicksPerSecond) / TicksPerMillisecond; 
	if(dateTime < 0 || value1 > (NLong)(((NULong)((time_t)-1)) / 2))
		return N_E_OVERFLOW;
	if(dateTime - value1 * TicksPerSecond - value2 * TicksPerMillisecond >= (TicksPerMillisecond / 2))
	{
		if (++value2 == 1000)
		{
			value2 = 0;
			if (value1 == (NLong)(((NULong)((time_t)-1)) / 2)) return N_E_OVERFLOW;
			value1++;
		}
	}
	*time = (time_t) value1;
	*miliseconds = (NInt)value2;

	return N_OK;
}

NChar * formatTime(NLong dateTime)
{
	NChar * buffer = NULL;
	time_t time;
	struct tm  *ts;
	NInt miliseconds;
	NChar format[256];

	if (NFailed(decodeTime(dateTime, &time, &miliseconds)))
	{
		return NULL;
	}

	buffer = malloc(sizeof(NChar) * 256);
	if (!buffer)
		return NULL;

	_sntprintf(format, 256, N_T("%%Y-%%m-%%d %%H:%%M:%%S:%ld"), (long)miliseconds);

	ts = localtime(&time);
	_tcsftime(buffer, 256, format, ts);
	
	return buffer;
}

#ifndef N_WINDOWS

void Sleep(unsigned long milliseconds)
{
	struct timespec t, r;
	t.tv_sec = milliseconds / 1000;
	t.tv_nsec = (milliseconds % 1000) * 1000000;
	while (nanosleep(&t, &r) == -1)
	{
		t = r;
	}
}

#ifdef N_UNICODE
FILE * wfopen(const wchar_t * filename, const wchar_t * access)
{
	char mbsfilename[MAXPATHLEN];
	char mbsaccess[MAXPATHLEN];

	wcstombs(mbsfilename, filename, MAXPATHLEN);
	wcstombs(mbsaccess, access, MAXPATHLEN);

	return fopen(mbsfilename, mbsaccess);
}

#if defined(N_MAC)
wchar_t * wcsdup(const wchar_t * wcstr)
{
	wchar_t * tstr = NULL;
	size_t slen = wcslen(wcstr);

	tstr = malloc((slen + 1) * sizeof(wchar_t));
	if (tstr == NULL)
	{
		return NULL;
	}

	return wcscpy(tstr, wcstr);
}
#endif
#endif

#include <stdarg.h>

static inline int _vftprintf(FILE *stream, const NChar * format, va_list ap)
{
	int ret = 0;

#ifdef N_UNICODE
	NChar * dupFormat = NULL;
	NChar * fixedFormat = NULL;
	NChar * tmpStr = NULL;
	NChar * stStr = NULL;

	fixedFormat = malloc((wcslen(format)+1)*sizeof(NChar)*2);
	if (fixedFormat == NULL)
	{
		fwprintf(stderr, N_T("Out of memmory"));
		va_end(ap);
		return -1;
	}

	dupFormat = wcsdup(format);

	tmpStr = dupFormat;

	wcscpy(fixedFormat, wcstok(tmpStr, N_T("%"), &stStr));

	for (tmpStr = wcstok(NULL, N_T("%"), &stStr); tmpStr != NULL; tmpStr = wcstok(NULL, N_T("%"), &stStr))
	{
		int i;

		wcscat(fixedFormat, N_T("%"));
		switch(tmpStr[0])
		{
		case N_T('#'):
		case N_T('-'):
		case N_T('+'):
		case N_T('\''):
		case N_T(' '):
			wcsncat(fixedFormat, tmpStr, 1);
			tmpStr = &tmpStr[1];
			break;

		}

		for (i = 0; i < wcslen(tmpStr); i++, tmpStr = &tmpStr[1])
		{
			if (iswdigit(tmpStr[0]))
			{
				wcsncat(fixedFormat, tmpStr, 1);
				continue;
			}

			break;
		}

		if (tmpStr[0] == N_T('.'))
		{
			wcsncat(fixedFormat, tmpStr, 1);
			tmpStr = &tmpStr[1];
		}

		for (i = 0; i < wcslen(tmpStr); i++, tmpStr = &tmpStr[1])
		{
			if (iswdigit(tmpStr[0]))
			{
				wcsncat(fixedFormat, tmpStr, 1);
				continue;
			}

			break;
		}

		if (tmpStr[0] == N_T('s'))
		{
			wcscat(fixedFormat, N_T("l"));
		}

		wcscat(fixedFormat, tmpStr);
	}

	free(dupFormat);

	ret = vfwprintf(stream, fixedFormat, ap);

	free(fixedFormat);
#else
	ret = vfprintf(stream, format, ap);
#endif
	return ret;

}

int _ftprintf(FILE *stream, const NChar *format, ...)
{
	va_list ap;
	int ret = 0;

	va_start(ap, format);

	ret = _vftprintf(stream, format, ap);

	va_end(ap);

	return ret;
}

int _tprintf(const NChar *format, ...)
{
	va_list ap;
	int ret = 0;

	va_start(ap, format);

	ret = _vftprintf(stdout, format, ap);

	va_end(ap);

	return ret;
}

#endif
