require 'rspec'
require_relative '../lib/mqwatch_analyzer'
require_relative '../lib/record'
require_relative '../lib/result_stream'

describe MQWatchAnalyzer do	
	let(:result_stream){
		ResultStream.new
	}
	let (:analyzer) { 
		MQWatchAnalyzer.new 100, result_stream 
	}
	it "should display empty line no input recorded" do
		result_stream.last_line.should == ""
	end	

	it "should display empty line for messages appearing during only first minute of an hour" do
		analyzer.analyze Record.new date:"1980.01.01 01:00",count:100
		result_stream.last_line.should == ""
	end	
	
	it "should display empty line for messages appearing during only first minutes of an hour" do
		analyzer.analyze Record.new date:"1980.01.01 01:00",count:100
		analyzer.analyze Record.new date:"1980.01.01 01:01",count:100 
		result_stream.last_line.should == ""
	end	

	it "should display empty line for single record on last minute of an hour" do
		analyzer.analyze Record.new date:"1980.01.01 01:59",count:100
		result_stream.last_line.should == ""
	end	

	it "should display manus flag for an hour where messages appeared during first and the last minute" do
		analyzer.analyze Record.new date:"1980.01.01 01:00",count:100
		analyzer.analyze Record.new date:"1980.01.01 01:59",count:100 
		result_stream.last_line.should == "1980.01.01 01:00 1"
	end	

	it "should display empty line for records not containing first minute of an hour" do
		analyzer.analyze Record.new date:"1980.01.01 01:55",count:100
		analyzer.analyze Record.new date:"1980.01.01 01:59",count:100
		result_stream.last_line.should == ""
	end	

	it "should display correct datetime for records containing first and last minute of any hour" do
		analyzer.analyze Record.new date:"1980.01.01 03:00",count:100
		analyzer.analyze Record.new date:"1980.01.01 03:59",count:100 
		result_stream.last_line.should == "1980.01.01 03:00 1"
	end	

	it "should display empty line for an hour where messages appeared during first and the last minute and value for the last minute doesn't reach threshold" do
		analyzer.analyze Record.new date:"1980.01.01 01:00",count:100
		analyzer.analyze Record.new date:"1980.01.01 01:59",count:99 
		result_stream.last_line.should == ""
	end	
	# it "should generate line for that hour" do
	# 	analyzer.analyze Record.new{date:"1980.1.01 1:00",count:100} 
	# 	analyzer.analyze Record.new{date:"1980.1.01 1:59",count:100}
	# 	analyzer.analyze Record.new{date:"1980.1.01 2:00",count:100}
	# 	analyzer.analyze Record.new{date:"1980.1.01 2:59",count:100}
	# 	result_stream.last_line.should == "1980.1.01 2:00 1"
	# end	
end