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
   fontSize = 20,
   font = "Helvetica",
   x = display.contentCenterX,
   y = 20,
   maxDigits = 7,
   leadingZeros = true,
   filename = "scorefile.txt",
})


contW = display.contentWidth
contH = display.contentHeight
centerX = display.contentCenterX
centerY = display.contentCenterY
pictureSize = 160;

local lionButton = display.newImageRect("lion.jpg", pictureSize, pictureSize);
lionButton.x = centerX - 85;
lionButton.y = centerY - 120;
local leopardButton = display.newImageRect("leopard.jpg", pictureSize, pictureSize);
leopardButton.x = centerX + 85;
leopardButton.y = centerY - 120;
local dogButton = display.newImageRect("dog.jpg", pictureSize, pictureSize);
dogButton.x = centerX - 85;
dogButton.y = centerY + 120;
local apeButton = display.newImageRect("ape.jpg", pictureSize, pictureSize);
apeButton.x = centerX + 85;
apeButton.y = centerY + 120;
--local myText = display.newText( "ello battohn", contW/2, 50, native.systemFont, 30)

local function mainButtonHandler(event)
        
        if(event.phase == "began") then
            --myText.text = "Button Phase is: " .. event.phase
            event.target.xScale = 0.85;
            event.target.yScale = 0.85;
        elseif(event.phase == "moved") then
            --myText.text = "Button Phase is: " .. event.phase
        elseif(event.phase == "ended" or event.phase == "cancelled") then
            --myText.text = "Button Phase is: " .. event.phase
            event.target.xScale = 1;
            event.target.yScale = 1;
        end
        
        return true
end

        
lionButton:addEventListener("touch", mainButtonHandler);
dogButton:addEventListener("touch", mainButtonHandler);
leopardButton:addEventListener("touch", mainButtonHandler);
apeButton:addEventListener("touch", mainButtonHandler);
    
local scoreSet = score.add(1);