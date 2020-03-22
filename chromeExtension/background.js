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
	var reasons = '';
	var verifiedUrl = '';
	if(data.fakeReasons && data.fakeReasons[0] && (data.fakeReasons[0].includes('html') || data.fakeReasons[0].includes('www')))
	{
		verifiedUrl = data.fakeReasons[0];
		reasons = " Naciśnij tu by przejść do zweryfikowanego źródła.";
	}
	else if (data.fakeReasons && data.fakeReasons[0])
	{
		reasons = " Powód: ".concat(data.fakeReasons[0]);
	}
	switch(data.fakebility) {
	case 1:
    notificationOption = {
		type: "basic",
		title: "Uważaj! Nierzetelne źródło",
		message: "Strona zawiera nieprawdziwe informacje!".concat(reasons),
		iconUrl: "/images/alert.png",
		requireInteraction: true  
	};
    break;
	case 2:
    notificationOption = {
		type: "basic",
		title: "Zachowaj ostrożność! Niezweryfikowane źródło",
		message: "Strona może zawierać niepotwierdzone informacje!".concat(reasons),
		iconUrl: "/images/warning.png",
		requireInteraction: true  
	};
		break;
	case 3:
    notificationOption = {
		type: "basic",
		title: "Zweryfikowane źródło",
		message: "Super! Strona którą przeglądasz zawiera potwierdzone informacje!".concat(reasons),
		iconUrl: "/images/ok.png",
		requireInteraction: true  
	};
		break;
	default:
		break;
	}
	if(notificationOption)
	{
		var notificationId = Math.random().toString();
			if(verifiedUrl)
			{
				var openNotificationUrl = function ()
				{
					chrome.tabs.create({url: verifiedUrl});
					chrome.notifications.onClicked.removeListener(openNotificationUrl);
				}
				chrome.notifications.create(notificationId,notificationOption,function(verifiedUrl){});
				chrome.notifications.onClicked.addListener(openNotificationUrl);	 
			}
		else
		{
			chrome.notifications.create('',notificationOption);
		}	
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
  
  
  
  
