build-release:
	git subtree split --prefix=BaseProject/Assets/PusherWebsocketUnity --branch upm

push-release:
	git push origin upm
