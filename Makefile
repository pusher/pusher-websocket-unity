build-release:
	git subtree split --prefix=BaseProject/Assets/PusherWebsocketUnity --branch upm
	git tag $(VERSION) upm -f

push-release:
	git push origin upm --tags -f
