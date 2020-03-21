 var site_url;
 var site_html;
 
  chrome.storage.local.get('site_url', function(data) {
    site_url = data.site_url;
  });
  
  chrome.storage.local.get('site_html', function(data) {
    site_html = data.site_html;
  });  
  
  let sendReport = document.getElementById('sendReport');  //TODO; refactor foreach
  sendReport.onclick = function(element) {
	var reason_arr = [];
	if(document.getElementById('reportReason1').value)
	{
		reason_arr.push(document.getElementById('reportReason1').value);
	}
	if(document.getElementById('reportReason2').value)
	{
		reason_arr.push(document.getElementById('reportReason2').value);
	}
	if(document.getElementById('reportReason3').value)
	{
		reason_arr.push(document.getElementById('reportReason3').value);
	}
  	chrome.runtime.sendMessage({contentScriptQuery: "MarkAsFake",
	url: site_url,
	html:site_html,
	fakeReasons:reason_arr
	},
	function(response) {
	 console.log(response.resp);
	 });
	document.getElementById('report-form').style.display = "none";
	document.getElementById('main-screen').style.display = "none";
	document.getElementById('report-sended').style.display = "block";
  };