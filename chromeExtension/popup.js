 var site_url;
 var site_html;
 
  chrome.storage.local.get('site_url', function(data) {
    site_url = data.site_url;
  });
  
  chrome.storage.local.get('site_html', function(data) {
    site_html = data.site_html;
  });
 
 let reportSite = document.getElementById('reportSite'); 
  reportSite.onclick = function(element) {
	chrome.runtime.sendMessage({contentScriptQuery: "MarkAsFake", url: site_url,html:site_html,fakeReasons:["testreson1","testreason2"]},function(response) {
	 console.log(response.resp);
	 });
  };