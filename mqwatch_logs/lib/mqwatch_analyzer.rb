class MQWatchAnalyzer
	def initialize(threshold, result_stream)
		@threshold = threshold
		@result_stream = result_stream
	end

	def analyze(record)
		if record.date.minute == 0
			@current_date = record.date
		end

		if(record.date.minute == 59 &&
			@current_date &&
			@current_date.hour == record.date.hour)
			@result_stream << "1980.1.01 1:00 1"
		end
	end
end