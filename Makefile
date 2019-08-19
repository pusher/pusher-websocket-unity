build-release:
	git subtree split --prefix=BaseProject/Assets/PusherWebsocketUnity --branch upm
	git tag $(VERSION) upm

push-release:
	git push origin upm --tags
