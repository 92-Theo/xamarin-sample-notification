# xamarin-sample-notification
xamarin notification sample with FCM


# FCM http body format
{
  "notification": 
  {
    "title": "Your Title",
    "body": "Your Text",
    "sound": "default",
    "click_action": "MAIN"
  },
  "data":{
	  "mykey":"myvalue"
  },
  "priority":"high",
  "to" : "yourDeviceToken"
}
