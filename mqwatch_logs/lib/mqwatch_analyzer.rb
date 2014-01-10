class MQWatchAnalyzer
	def initialize(threshold, result_stream)
		@threshold = threshold
		@result_stream = result_stream
	end

	def analyze(record)
		if record.count < @threshold
			@current_date = nil 
		elsif record.date.minute == 0
			@current_date = record.date
		end

		if(record.date.minute == 59 &&
			@current_date &&
			@current_date.hour == record.date.hour)
			@result_stream << "#{@current_date.strftime("%Y.%m.%d %H:%M")} 1"
		end
	end
end