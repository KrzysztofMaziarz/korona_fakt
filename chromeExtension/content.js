var site_url = window.location.href; 
var site_html = $("html").prop('outerHTML');
 
 chrome.runtime.sendMessage({contentScriptQuery: "GetFakebility", url: site_url,html:site_html},function(response) {
	 console.log(response.resp);
	 });
	
