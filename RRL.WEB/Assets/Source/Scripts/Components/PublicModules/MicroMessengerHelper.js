import $ from 'jquery';

const microMessenger = function(){
	this.getOpenId = function(){
		$.get('/SetOpenIdForWx');
	};
};

export default microMessenger;