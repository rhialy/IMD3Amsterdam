-----------------------------------------------------------------------------------------
--
-- main.lua
--
-----------------------------------------------------------------------------------------

-- Your code here


display.setStatusBar(display.HiddenStatusBar);

local composer = require ( "composer" );
--composer.gotoScene( "start" );

local score = require( "score" )

local scoreText = score.init({
   fontSize = 14,
   font = "Helvetica",
   x = display.contentCenterX,
   y = -30,
   maxDigits = 7,
   leadingZeros = true,
   filename = "scorefile.txt",
})


contW = display.contentWidth
contH = display.contentHeight
centerX = display.contentCenterX
centerY = display.contentCenterY
pictureSize = 160;
checkMarkSize = 50;
nextButtonSize = 70;
isChecked = false;
checkedImageNumber = 0;



local lionButton = display.newImageRect("lion.jpg", pictureSize, pictureSize);
lionButton.x = centerX - 85;
lionButton.y = centerY - 150;
local leopardButton = display.newImageRect("leopard.jpg", pictureSize, pictureSize);
leopardButton.x = centerX + 85;
leopardButton.y = centerY - 150;
local dogButton = display.newImageRect("testPic.png", pictureSize, pictureSize);
dogButton.x = centerX - 85;
dogButton.y = centerY + 80;
local apeButton = display.newImageRect("ape.jpg", pictureSize, pictureSize);
apeButton.x = centerX + 85;
apeButton.y = centerY + 80;
--local myText = display.newText( "ello battohn", contW/2, 50, native.systemFont, 30)
local nextButton = display.newImageRect("testButton.png", nextButtonSize, nextButtonSize);
nextButton.x = centerX;
nextButton.y = centerY + 210
nextButton:rotate(90);



local function checkingIfChecked(object)
    
    if(isChecked == true or checkedImageNumber < 3) then
        checkButton = display.newImageRect("checkmarck.png", checkMarkSize, checkMarkSize);
        object:setFillColor( 0.55, 0.55, 0.55, 0.95);
        checkButton.x = object.x;
        checkButton.y = object.y;
        --object:translate( 1,0);
        --object:translate(-1,0);
    elseif(isChecked == false) then
        object:setFillColor( 1, 1, 1, 1);
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
            
            if(isChecked == false) then
                isChecked = true;
                checkedImageNumber = checkedImageNumber + 1;
            elseif(isChecked == true) then
                isChecked = false;
                checkedImageNumber = checkedImageNumber - 1;
            end

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
    
local scoreSet = score.add(1);