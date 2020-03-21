var site_url = window.location.href; 
var site_html = $("html").prop('outerHTML');

chrome.storage.local.set({site_url: site_url});
chrome.storage.local.set({site_html: site_html});
 
chrome.runtime.sendMessage({contentScriptQuery: "GetFakebility", url: site_url,html:site_html},function(response) {
	 console.log(response.resp);
	 });
	