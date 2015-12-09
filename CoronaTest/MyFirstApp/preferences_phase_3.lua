-----------------------------------------------------------------------------------------
--
-- main.lua
--
-----------------------------------------------------------------------------------------

-- Your code here

local composer = require ( "composer" );

local pp3 = composer.newScene()

local lionButton;
local leopardButton;
local dogButton;
local apeButton;
local nextButton;

function pp3:create(event)
    
    print("Entered P-Phase 3");
    
    composer.removeScene("preferences_phase_2");
    
    local sceneGroup = self.view
    local nextButtonSize = 70;
    local pictureSize = 160;
    contW = display.contentWidth
    contH = display.contentHeight
    centerX = display.contentCenterX
    centerY = display.contentCenterY
    checkMarkSize = 50;
    checkedImageNumber = 0;
    
    
    lionButton = display.newImageRect("ape.jpg", pictureSize, pictureSize);
    lionButton.x = centerX - 85;
    lionButton.y = centerY - 150;
    leopardButton = display.newImageRect("ape.jpg", pictureSize, pictureSize);
    leopardButton.x = centerX + 85;
    leopardButton.y = centerY - 150;
    dogButton = display.newImageRect("ape.jpg", pictureSize, pictureSize);
    dogButton.x = centerX - 85;
    dogButton.y = centerY + 80;
    apeButton = display.newImageRect("ape.jpg", pictureSize, pictureSize);
    apeButton.x = centerX + 85;
    apeButton.y = centerY + 80;
    --local myText = display.newText( "ello battohn", contW/2, 50, native.systemFont, 30)
    nextButton = display.newImageRect("testButton.png", nextButtonSize, nextButtonSize);
    nextButton.x = centerX;
    nextButton.y = centerY + 210
    nextButton:rotate(90);
    
    sceneGroup:insert(lionButton);
    sceneGroup:insert(leopardButton);
    sceneGroup:insert(dogButton);
    sceneGroup:insert(apeButton);
    sceneGroup:insert(nextButton);
 
end


function pp3:show(event)
    
    local sceneGroup = self.view;
    local phase = event.phase;
    
    if(phase == "did") then
        
        
        local function checkingIfChecked(object)

        if(object.alpha > 0.95) then
            checkButton = display.newImageRect("checkmarck.png", checkMarkSize, checkMarkSize);
            sceneGroup:insert(checkButton);
            object:setFillColor( 0.55, 0.55, 0.55);
            object.alpha = 0.95;
            checkButton.x = object.x;
            checkButton.y = object.y;
            --object:translate( 1,0);
            --object:translate(-1,0);
        elseif(object.alpha <= 0.95) then
            object:setFillColor( 1, 1, 1);
            object.alpha = 1;
            if(checkButton ~= nil) then
                checkButton:removeSelf()
            end
            --object:translate( 1,0);
            --object:translate(-1,0);

            end
        end


        local function mainButtonHandler(event)

                if(event.phase == "began") then  
                    event.target.xScale = 0.85;
                    event.target.yScale = 0.85;
                    --myText.text = "Button Phase is: " .. event.phase        
                elseif(event.phase == "moved") then
                    --myText.text = "Button Phase is: " .. event.phase
                elseif(event.phase == "ended" or event.phase == "cancelled") then
                    --myText.text = "Button Phase is: " .. event.phase
                    event.target.xScale = 1;
                    event.target.yScale = 1;
                    checkingIfChecked(event.target);
                end

                return true
        end
        
        lionButton:addEventListener("touch", mainButtonHandler);
        dogButton:addEventListener("touch", mainButtonHandler);
        leopardButton:addEventListener("touch", mainButtonHandler);
        apeButton:addEventListener("touch", mainButtonHandler);


        local function nextScene(event)

            if(event.phase =="ended" or event.phase == "cancelled") then
                composer.gotoScene("preferences_phase_1");
            end
        end

        nextButton:addEventListener("touch", nextScene);
    end
end


function pp3:hide( event )

    local sceneGroup = self.view
    local phase = event.phase

    if ( phase == "will" ) then

    elseif ( phase == "did" ) then

    end
end


function pp3:destroy( event )

    local sceneGroup = self.view
    sceneGroup:remove(lionButton);
    sceneGroup:remove(leopardButton);
    sceneGroup:remove(dogButton);
    sceneGroup:remove(apeButton);
    
end

pp3:addEventListener("create", scene);
pp3:addEventListener("show", scene);
pp3:addEventListener("hide", scene);
pp3:addEventListener("destroy", scene);

return pp3;