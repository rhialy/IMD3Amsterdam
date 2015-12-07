local composer = require( "composer" )

local scene = composer.newScene()

function scene:create( event )

    local sceneGroup = self.view

    -- Initialize the scene here.
    -- Example: add display objects to "sceneGroup", add touch listeners, etc.
end

scene:addEventListener( "create", scene )

return scene