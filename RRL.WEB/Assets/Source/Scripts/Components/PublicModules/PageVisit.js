import $ from 'jquery';

let visitStartTime = null;
let visitData = null;

const PageVisit = {
	logVisitTime(data) {
		this.visitData = data;
		this.visitStartTime = new Date().getTime();
	},
	logLeaveTime() {
		let visitEndTime = new Date().getTime();
		let data = {
			token: localStorage.getItem("shorttoken"),
			visitTime: this.visitStartTime,
			leaveTime: visitEndTime,
			visitInterval: Math.round((visitEndTime - this.visitStartTime) / 1000), // 转换成秒
			pageType: this.visitData.pageType,
			goodsId: this.visitData.goodsId,
			visitPath: this.visitData.visitPath,
			deep: this.visitData.deep
		};
		this.logVisitData(data);
	},
	logPageVisitOnly(data){
		this.logVisitData(data);
	},
	logVisitData(data) {
		$.get('/WebApi/VisitStatistics/RecordsVisitLog', data);
	}
};

export default PageVisit;