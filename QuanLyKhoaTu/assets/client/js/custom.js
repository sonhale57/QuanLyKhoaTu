(function () {

	'use strict'


	AOS.init({
		duration: 800,
		easing: 'slide',
		once: true
	});

	var rellax = new Rellax('.rellax');

	var preloader = function() {

		var loader = document.querySelector('.loader');
		var overlay = document.getElementById('overlayer');

		function fadeOut(el) {
			el.style.opacity = 1;
			(function fade() {
				if ((el.style.opacity -= .1) < 0) {
					el.style.display = "none";
				} else {
					requestAnimationFrame(fade);
				}
			})();
		};

		setTimeout(function() {
			fadeOut(loader);
			fadeOut(overlay);
		}, 200);
	};
	preloader();
	

	var tinyslier = function() {

		

		var el = document.querySelectorAll('.wide-slider-testimonial');
		if ( el.length > 0 ) {
			var slider = tns({
				container: ".wide-slider-testimonial",
				items: 1,
				slideBy: 1,
				axis: "horizontal",
				swipeAngle: false,
				speed: 700,
				nav: true,
				loop: true,
				edgePadding: 40,
				controls: true,
				controlsPosition: "bottom",
				autoHeight: true,
				autoplay: true,
				mouseDrag: true,
				autoplayHoverPause: true,
				autoplayTimeout: 3500,
				autoplayButtonOutput: false,
				controlsContainer: "#prevnext-testimonial",
				responsive: {
					350: {
						items: 1
					},
					
					500: {
						items: 2
					},
					600: {
						items: 3
					},
					900: {
						items: 5
					}
				},
			});
		}


		var destinationSlider = document.querySelectorAll('.destination-slider');

		if ( destinationSlider.length > 0 ) {
			var desSlider = tns({
				container: ".destination-slider",
				mouseDrag: true,
				items: 1,
				axis: "horizontal",
				swipeAngle: false,
				speed: 700,
				edgePadding: 50,
				nav: true,
				gutter: 30,
				autoplay: true,
				autoplayButtonOutput: false,
				controlsContainer: "#destination-controls",
				responsive: {
					350: {
						items: 1
					},
					
					500: {
						items: 2
					},
					600: {
						items: 3
					},
					900: {
						items: 5
					}
				},
			})
		}



	}
	tinyslier();


	var lightbox = function() {
		var lightboxVideo = GLightbox({
			selector: '.glightbox3'
		});
	};
	lightbox();


	const submitBtn = document.getElementById('save')

	const tsten = document.getElementById('ts_ten')
	const tsngaysinh = document.getElementById('ts_ngaysinh')
	const tsgioitinh = document.getElementById('ts_gioitinh')
	const tsemail = document.getElementById('ts_email')
	const tscmnd = document.getElementById('ts_cmnd')
	const tssdt = document.getElementById('ts_sdt')
	const tssdtnt = document.getElementById('ts_sdtnt')
	const lastcheck = document.getElementById('lastcheck')
	
	// run this function whenever the values of any of the above 4 inputs change.
	// this is to check if the input for all 4 is valid.  if so, enable submitBtn.
	// otherwise, disable it.
	const checkEnableButton = () => {
	  submitBtn.disabled = !(
		  tsten.value && 
		  tsngaysinh.value && 
		  tsgioitinh !== 'Choose' &&
		  tsemail.value && 
		  tscmnd.value && 
		  tssdt.value &&
		  tssdtnt.value && 
		  lastcheck.checked
	   )
	}
	
	tsten.addEventListener('change', checkEnableButton)
	tsngaysinh.addEventListener('change', checkEnableButton)
	tsgioitinh.addEventListener('change', checkEnableButton)
	tsemail.addEventListener('change', checkEnableButton)
	tscmnd.addEventListener('change', checkEnableButton)
	tssdt.addEventListener('change', checkEnableButton)
	tssdtnt.addEventListener('change', checkEnableButton)
	lastcheck.addEventListener('change', checkEnableButton)

	
})();

function isDiCung() {
	// Get the checkbox
	var checkBox = document.getElementById("is_dicung");
	// Get the output text
	var ts_dicung = document.getElementById("ts_dicung");
	
	// If the checkbox is checked, display the output text
	if (checkBox.checked == true){
	  ts_dicung.style.display = "block";
	} else {
	  ts_dicung.style.display = "none";
	}
	}


