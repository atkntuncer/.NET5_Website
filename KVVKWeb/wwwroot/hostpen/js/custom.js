/******************************************
    File Name: custom.js
    Template Name: HostPen
    Created By: PSD Convert HTML Team
    Envato Profile: http://themeforest.net/user/psdconverthtml
    Website: http://psdconverthtml.com/
    Version: 1.0
/******************************************/

(function($) {
    "use strict";

    /* ==============================================
     TOOLTIP -->
     =============================================== */

    $('[data-toggle="tooltip"]').tooltip()
    $('[data-toggle="popover"]').popover()

    /* ==============================================
    BACK TOP
    =============================================== */

    jQuery('.backtotop').on(function() {
        jQuery('html, body').animate({
            scrollTop: '0px'
        }, 800);
        return false;
    });

    /* ==============================================
     LOADER -->
     =============================================== */

    $(window).load(function() {
        $('#loader').delay(300).fadeOut('slow');
        $('#loader-container').delay(200).fadeOut('slow');
        $('body').delay(300).css({
            'overflow': 'visible'
        });
        
        $("#preloader").delay(500).fadeOut();
        $(".preloader").delay(600).fadeOut("slow");
    })

    /* ==============================================
     FUN FACTS -->
     =============================================== */

    function count($this) {
        var current = parseInt($this.html(), 10);
        current = current + 1; /* Where 50 is increment */
        $this.html(++current);
        if (current > $this.data('count')) {
            $this.html($this.data('count'));
        } else {
            setTimeout(function() {
                count($this)
            }, 50);
        }
    }
    $(".stat-count").each(function() {
        $(this).data('count', parseInt($(this).html(), 10));
        $(this).html('0');
        count($(this));
    });

    /* ==============================================
    AFFIX -->
    =============================================== */
    $('body.onepage .header').affix({
        offset: {
            top: 0,
            bottom: function() {
                return (this.bottom = $('.copyrights').outerHeight(true))
            }
        }
    })

})(jQuery);