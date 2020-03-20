var url = window.location.href; 
var html = $("html").html();
 
 chrome.runtime.sendMessage({contentScriptQuery: "getComments", postId: 1},function(response) {
	 console.log(response.comments);
	 });
	
