var tpj=jQuery;
var revapi420;
tpj(document).ready(function() {
if(tpj("#rev_slider_420_1").revolution == undefined){
revslider_showDoubleJqueryError("#rev_slider_420_1");
}else{
revapi420 = tpj("#rev_slider_420_1").show().revolution({
sliderType:"standard",
jsFileLocation:"/revslider/public/assets/js/",
dottedOverlay:"none",
delay:9000,
navigation: {
keyboardNavigation:"off",
keyboard_direction: "horizontal",
sliderLayout: "fullwidth",
mouseScrollNavigation:"off",
mouseScrollReverse:"default",
onHoverStop:"on",
touch:{
touchenabled:"on",
swipe_threshold: 75,
swipe_min_touches: 50,
swipe_direction: "horizontal",
drag_block_vertical: false
}
,
thumbnails: {
style:"gyges",
enable:false,
width:80,
height:80,
min_width:80,
wrapper_padding:10,
wrapper_color:"#333333",
wrapper_opacity:"0",
tmp:'<span class="tp-thumb-img-wrap">  <span class="tp-thumb-image"></span></span>',
visibleAmount:5,
hide_onmobile:false,
hide_onleave:false,
direction:"vertical",
span:true,
position:"inner",
space:10,
h_align:"right",
v_align:"center",
h_offset:0,
v_offset:0
}
},
responsiveLevels:[1240,1024,778,480],
visibilityLevels:[1240,1024,778,480],
gridwidth:[1200,1024,778,480],
gridheight:[600,600,600,450],
lazyType:"single",
parallax: {
type:"scroll",
origo:"slidercenter",
speed:400,
levels:[5,10,15,20,25,30,35,40,45,46,47,48,49,50,51,55],
type:"scroll",
},
shadow:0,
spinner:"off",
stopLoop:"off",
stopAfterLoops:-1,
stopAtSlide:-1,
shuffle:"off",
autoHeight:"off",
hideThumbsOnMobile:"off",
hideSliderAtLimit:0,
hideCaptionAtLimit:0,
hideAllCaptionAtLilmit:0,
debugMode:false,
fallbacks: {
simplifyAll:"off",
nextSlideOnWindowFocus:"off",
disableFocusListener:false,
}
});
}
}); /*ready*/ 