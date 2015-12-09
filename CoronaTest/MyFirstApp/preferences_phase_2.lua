-----------------------------------------------------------------------------------------
--
-- main.lua
--
-----------------------------------------------------------------------------------------

-- Your code here

local composer = require ( "composer" );

local pp2 = composer.newScene()

local lionButton;
local leopardButton;
local dogButton;
local apeButton;
local nextButton;
local checkedImageNumber;
local checkMarkSize;

function pp2:create(event)
    
    print("Entered P-Phase 2");
    
    composer.removeScene("preferences_phase_1");
    
    local sceneGroup = self.view
    local nextButtonSize = 70;
    local pictureSize = 160;
    contW = display.contentWidth
    contH = display.contentHeight
    centerX = display.contentCenterX
    centerY = display.contentCenterY
    checkMarkSize = 50;
    checkedImageNumber = 0;
    
    
    lionButton = display.newImageRect("leopard.jpg", pictureSize, pictureSize);
    lionButton.x = centerX - 85;
    lionButton.y = centerY - 150;
    leopardButton = display.newImageRect("leopard.jpg", pictureSize, pictureSize);
    leopardButton.x = centerX + 85;
    leopardButton.y = centerY - 150;
    dogButton = display.newImageRect("leopard.jpg", pictureSize, pictureSize);
    dogButton.x = centerX - 85;
    dogButton.y = centerY + 80;
    apeButton = display.newImageRect("leopard.jpg", pictureSize, pictureSize);
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


function pp2:show(event)
    
    local sceneGroup = self.view;
    local phase = event.phase;
    
    if(phase == "did") then
        
        local function checkHandler(object)
            if(checkedImageNumber == 1) then
                firstCheck = display.newImageRect("checkmarck.png", checkMarkSize, checkMarkSize);
                firstCheck.x = object.x;
                firstCheck.y = object.y;
                sceneGroup:insert(firstCheck);
            end
            if(checkedImageNumber == 2) then
                secondCheck = display.newImageRect("checkmarck.png", checkMarkSize, checkMarkSize);
                secondCheck.x = object.x;
                secondCheck.y = object.y;
                sceneGroup:insert(secondCheck);
            end
        end
        
        local function checkDestroyer(object)
            if(checkedImageNumber == 0) then
                if(firstCheck ~= nil) then
                    firstCheck:removeSelf();
                    firstcheck = nil;
                end
            end
            if(checkedImageNumber == 1) then
                print("is in conditional");
                if(secondCheck ~= nil) then
                    secondCheck:removeSelf();
                    secondCheck = nil;
                end
            end
        end

        
        local function checkingIfChecked(object)
            
            if(object.alpha > 0.95) then
                if(checkedImageNumber < 2) then
                    object:setFillColor( 0.55, 0.55, 0.55);
                    object.alpha = 0.95;
                    checkedImageNumber = checkedImageNumber + 1;
                    checkHandler(object);
                end
                    
                --object:translate( 1,0);
                --object:translate(-1,0);
            elseif(object.alpha <= 0.95) then
                
                object:setFillColor( 1, 1, 1);
                object.alpha = 1;
                
                checkedImageNumber = checkedImageNumber - 1;
                checkDestroyer(object);
                --object:translate( 1,0);
                --object:translate(-1,0);

            end
            print("checkedImageNumber :" and tostring(checkedImageNumber));
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
                if(checkedImageNumber > 0) then
                    composer.gotoScene("preferences_phase_3");
                end
            end
        end

        nextButton:addEventListener("touch", nextScene);
    end
end


function pp2:hide( event )

    local sceneGroup = self.view
    local phase = event.phase

    if ( phase == "will" ) then

    elseif ( phase == "did" ) then

    end
end


function pp2:destroy( event )

    local sceneGroup = self.view
    --sceneGroup:remove(lionButton);
    --sceneGroup:remove(leopardButton);
    --sceneGroup:remove(dogButton);
    --sceneGroup:remove(apeButton);
    
    
end

pp2:addEventListener("create", scene);
pp2:addEventListener("show", scene);
pp2:addEventListener("hide", scene);
pp2:addEventListener("destroy", scene);

return pp2;