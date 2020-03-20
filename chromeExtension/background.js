//Listner czekajacy na event z content.js. Pośredniczy w komunikacji z api 
  chrome.runtime.onMessage.addListener(
  function(request, sender, sendResponse) {
    if (request.contentScriptQuery == "getComments") {
      var url = "https://jsonplaceholder.typicode.com/comments?postId=" + encodeURIComponent(request.postId); //Przykładowe API, wykonywany jest get z parametrem
      fetch(url)
		  .then(response => response.json())
          .then(function(data)
		  {
			sendResponse({comments:data}) //zwracany jest response
		  })
      return true;  // Will respond asynchronously.
    }
  });