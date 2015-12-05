function getBarcodeInfo(strUrl)
{

    // GLN	1000000000009	GS1 US
    // GLN	6270000000001	GS1 Kuwait

    var strResult;

    var xml = "<?xml version=\"1.0\"?><root/>"  //just an example
        xml = '<?xml version="1.0"?><root/>'    //just an example

    //GetItemByGTIN	SOAP 1.2
    xml = ''
	+'<?xml version="1.0" encoding="utf-8"?>'
	+ '<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">'
  	+ '  <soap12:Header>'
	+ '    <gepirRequestHeader xmlns="http://www.gepir.org/">'
	+ '      <requesterGln>6270000000001</requesterGln>'
	+ '      <cascade>9</cascade>'
	+ '    </gepirRequestHeader>'
	+ '  </soap12:Header>'
	+ '  <soap12:Body>'
	+ '    <GetItemByGTIN version="1" xmlns="http://www.gepir.org/">'
	+ '      <requestedGtin>6271021090002</requestedGtin>'
	+ '      <requestedLanguages>'
	+ '        <language>en</language>'
	+ '      </requestedLanguages>'
	+ '    </GetItemByGTIN>'
	+ '  </soap12:Body>'
	+ '</soap12:Envelope>';

    //GetItemByGTIN	SOAP 1.1
    xml11 = ''
	+ '<?xml version="1.0" encoding="utf-8"?>'
	+ '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'
	+ '  <soap:Header>'
	+ '    <gepirRequestHeader xmlns="http://www.gepir.org/">'
	+ '      <requesterGln>1000000000009</requesterGln>'
	+ '      <cascade>9</cascade>'
	+ '    </gepirRequestHeader>'
	+ '  </soap:Header>'
	+ '  <soap:Body>'
	+ '    <GetItemByGTIN version="31" xmlns="http://www.gepir.org/">'
	+ '      <requestedGtin>6271021090002</requestedGtin>'
	+ '      <requestedLanguages>'
	+ '        <language>en</language>'
	+ '        <language>en</language>'
	+ '      </requestedLanguages>'
	+ '    </GetItemByGTIN>'
	+ '  </soap:Body>'
	+ '</soap:Envelope>';



    //GepirVersion2Request	SOAP 1.2
    xml12 = ""
	+ '<?xml version="1.0" encoding="utf-8"?>'
	+ '<soap12:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap12="http://www.w3.org/2003/05/soap-envelope">'
	+ '  <soap12:Body>'
	+ '    <GepirVersion2Request xmlns="http://gepir.ean.ch/Request" />'
	+ '  </soap12:Body>'
	+ '</soap12:Envelope>';

    //GepirVersion2Request	SOAP 1.1 ------------------------------ does not work for this service
    xml11 = ""	
	+ '<?xml version="1.0" encoding="utf-8"?>'
	+ '<soap:Envelope xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" xmlns:soap="http://schemas.xmlsoap.org/soap/envelope/">'
	+ '  <soap:Body>'
	+ '    <GepirVersion2Request xmlns="http://gepir.ean.ch/Request" />'
	+ '  </soap:Body>'
	+ '</soap:Envelope>';

    try
    {
	// Create the WinHTTPRequest ActiveX Object.
        var WinHttpReq = new ActiveXObject("WinHttp.WinHttpRequest.5.1");

	WinHttpReq.Open("POST", strUrl, false);

	WinHttpReq.SetRequestHeader("Host", "gepir.gs1.org") 					
//	WinHttpReq.SetRequestHeader("Content-Type", "text/xml; charset=utf-8") 			//SOAP 1.1
	WinHttpReq.SetRequestHeader("Content-Type", "application/soap+xml; charset=utf-8")	//SOAP 1.2
//	WinHttpReq.SetRequestHeader("SOAPAction", "http://www.gepir.org/GetItemByGTIN")		//SOAP 1.1
//	WinHttpReq.SetRequestHeader("SOAPAction", "http://gepir.ean.ch/GepirVersion2")		//SOAP 1.1

	//  Send the HTTP request.
        WinHttpReq.Send(xml);


	//  Retrieve the HTTP status code ( should be 200 )	
        strResult = WinHttpReq.Status;

	//  Retrieve the status text ( should be OK )	
        strResult = WinHttpReq.StatusText;

	//  Retrieve the response entity body as an array of unsigned bytes
        strResult = WinHttpReq.ResponseBody;

	//  Retrieve the response entity body as text
        strResult = WinHttpReq.ResponseText;


    }
    catch (objError)
    {
        strResult = objError + "\n"
        strResult += "WinHTTP returned error: " + 
            (objError.number & 0xFFFF).toString() + "\n\n";
        strResult += objError.description;
    }
    
    //  Return the response text.
    return strResult;
}

WScript.Echo(getBarcodeInfo("http://gepir.gs1.org/V31/router.asmx?WSDL"));

