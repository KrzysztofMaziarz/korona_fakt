chrome.runtime.onInstalled.addListener(function() {
    chrome.storage.local.set({site_url: 'http://example.com'});
	chrome.storage.local.set({site_html: 'sample html'});
    chrome.declarativeContent.onPageChanged.removeRules(undefined, function() {
      chrome.declarativeContent.onPageChanged.addRules([{
        conditions: [new chrome.declarativeContent.PageStateMatcher({
          pageUrl: {hostEquals: 'developer.chrome.com'},
        })
        ],
            actions: [new chrome.declarativeContent.ShowPageAction()]
      }]);
    });
  });

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
	var notificationOption;
	var reasons = data.fakeReasons.join('<br>');
	switch(data.fakebility) {
	case 1:
    notificationOption = {
		type: "basic",
		title: "Nierzetelne źródło",
		message: "Uważaj! Strona którą przeglądasz zawiera informacje zgłoszone jako nieprawdziwe lub nierzetelne! Powód:<br>".concat(reasons),
		iconUrl: "/images/alert.png",
		requireInteraction: true  
	};
    break;
	case 2:
    notificationOption = {
		type: "basic",
		title: "Nieweryfikowane źródło",
		message: "Zachowaj ostrożność! Strona którą przeglądasz nie została zweryfikowana i może zawierać niepotwierdzone informacje! Powód:<br>".concat(reasons),
		iconUrl: "/images/warning.png",
		requireInteraction: true  
	};
		break;
	case 3:
    notificationOption = {
		type: "basic",
		title: "Zweryfikowane źródło",
		message: "Super! Strona którą przeglądasz zawiera potwierdzone informacje!",
		iconUrl: "/images/ok.png",
		requireInteraction: true  
	};
		break;
	default:
		break;
	}
	if(notificationOption)
	{
			chrome.notifications.create('',notificationOption);
	}
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
  
  
  
  
