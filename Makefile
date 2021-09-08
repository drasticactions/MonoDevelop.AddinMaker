CONFIG?=Release
PROJECT_NAME=MonoDevelop.AddinMaker
FRAMEWORK_FOLDER=net472
VERSION=1.7.0

VS_PATH?=/Applications/Visual\ Studio\ \(Preview\).app
VS_DEBUG_PATH?=../vsmac/main/build/bin/VisualStudio.app

all: restore
	msbuild ${PROJECT_NAME}.sln /p:Configuration=${CONFIG} ${ARGS}

clean:
	msbuild ${PROJECT_NAME}.sln /p:Configuration=${CONFIG} /t:Clean ${ARGS}

install: restore
	mono $(VS_PATH)/Contents/MonoBundle/vstool.exe setup pack ./${PROJECT_NAME}/bin/${CONFIG}/${FRAMEWORK_FOLDER}/${PROJECT_NAME}.dll
	mono $(VS_PATH)/Contents/MonoBundle/vstool.exe setup install ./${PROJECT_NAME}/bin/${CONFIG}/${FRAMEWORK_FOLDER}/${PROJECT_NAME}_${VERSION}.mpack

install_debug: restore
	mono $(VS_DEBUG_PATH)/Contents/MonoBundle/vstool.exe setup pack ./${PROJECT_NAME}/bin/${CONFIG}/${FRAMEWORK_FOLDER}/${PROJECT_NAME}.dll
	mono $(VS_DEBUG_PATH)/Contents/MonoBundle/vstool.exe setup install ./${PROJECT_NAME}/bin/${CONFIG}/${FRAMEWORK_FOLDER}/${PROJECT_NAME}_${VERSION}.mpack

uninstall: restore
	mono $(VS_PATH)/Contents/MonoBundle/vstool.exe setup uninstall MonoDevelop.AddinMaker

uninstall_debug: restore
	mono $(VS_DEBUG_PATH)/Contents/MonoBundle/vstool.exe setup uninstall MonoDevelop.AddinMaker

restore:
	msbuild ${PROJECT_NAME}.sln /t:Restore /p:Configuration=${CONFIG} ${ARGS}

.PHONY: all clean install restore
