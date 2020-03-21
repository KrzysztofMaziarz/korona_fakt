//Listner czekajacy na event z content.js. Pośredniczy w komunikacji z api 
  chrome.runtime.onMessage.addListener(
  function(request, sender, sendResponse) {
    if (request.contentScriptQuery == "GetFakebility") {
      
	fetch('https://localhost:5001/FakeNews/GetFakebility', {
    method: 'POST',
    body: JSON.stringify({
      UrlAddress: JSON.stringify(request.url),
      InnerHtml: JSON.stringify(request.html),
    }),
    headers: {
      "Content-type": "application/json; charset=UTF-8"
    }
  })
  .then(response => response.json())
  .then(function(data)
	{
			sendResponse({resp:data}) //zwracany jest response
	})
      return true;  // Will respond asynchronously.
    }
	if (request.contentScriptQuery == "MarkAsFake") {
      
	fetch('https://localhost:5001/FakeNews/MarkAsFake', {
    method: 'POST',
    body: JSON.stringify({
      UrlToMark: JSON.stringify(request.url),
      InnerHtml: JSON.stringify(request.html),
	  FakeReasons: request.fakeReasons,
    }),
    headers: {
      "Content-type": "application/json; charset=UTF-8"
    }
  })
  .then(response => response.json())
  .then(function(data)
	{
			sendResponse({resp:data}) //zwracany jest response
	})
      return true;  // Will respond asynchronously.
    }	
  });
  
  
  
  
