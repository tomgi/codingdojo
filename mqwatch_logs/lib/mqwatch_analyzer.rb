class MQWatchAnalyzer
	def initialize(threshold, result_stream)
		@threshold = threshold
		@result_stream = result_stream
	end

	def analyze(record)
		if @last_started_hour != nil && record.date.hour != @last_started_hour.hour
			@result_stream << "#{@last_started_hour.strftime("%Y.%m.%d %H:00")} 1"
			@last_started_hour = record.date
		end

		if record.count < @threshold
			@last_started_hour = nil 
		elsif record.date.minute == 0
			@last_started_hour = record.date
		end

		if(record.date.minute == 59 && @last_started_hour)
			@result_stream << "#{@last_started_hour.strftime("%Y.%m.%d %H:00")} 1"
		end
	end
end